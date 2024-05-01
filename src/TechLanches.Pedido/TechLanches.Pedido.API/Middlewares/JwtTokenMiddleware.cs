using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using TechLanches.Adapter.AWS.SecretsManager;
using System.Net;

namespace TechLanches.Pedido.API.Middlewares
{
    public class JwtTokenMiddleware : IMiddleware
    {
        private readonly TechLanchesCognitoSecrets _cognitoSecrets;

        public JwtTokenMiddleware(IOptions<TechLanchesCognitoSecrets> cognitoOptions)
        {
            _cognitoSecrets = cognitoOptions.Value;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // Get the token from the Authorization header
            var token = await context.GetTokenAsync("access_token")
                ?? context.Request.Headers.Authorization.ToString().Replace("Bearer ", "");

            if (!token.IsNullOrEmpty())
            {

                try
                {
                    // Verify the token using the JwtSecurityTokenHandlerWrapper
                    var claimsPrincipal = ValidateJwtToken(token);

                    // Extract the user ID from the token
                    var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    // Store the user ID in the HttpContext items for later use
                    context.Items["Cpf"] = userId;
                }
                catch (Exception)
                {
                    // If the token is invalid, throw an exception
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsync("Unauthorized");
                }
            }

            // Continue processing the request
            await next(context);
        }

        private ClaimsPrincipal ValidateJwtToken(string token)
        {
            try
            {
                // Create a token handler and validate the token
                var tokenHandler = new JwtSecurityTokenHandler();
                var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _cognitoSecrets.CognitoUri,
                    ValidateLifetime = true,
                    LifetimeValidator = (before, expires, token, param) => expires > DateTime.UtcNow,
                    ValidateAudience = false
                }, out SecurityToken validatedToken);

                // Return the claims principal
                return claimsPrincipal;
            }
            catch (SecurityTokenExpiredException)
            {
                // Handle token expiration
                throw new ApplicationException("Token has expired.");
            }
        }
    }
}