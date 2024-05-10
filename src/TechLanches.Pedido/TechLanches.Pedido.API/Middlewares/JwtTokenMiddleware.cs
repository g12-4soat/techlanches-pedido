using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using TechLanches.Adapter.AWS.SecretsManager;
using System.Net;
using Microsoft.Extensions.Caching.Memory;
using TechLanches.Application.Constantes;

namespace TechLanchesPedido.API.Middlewares
{
    public class JwtTokenMiddleware : IMiddleware
    {
        private readonly TechLanchesCognitoSecrets _cognitoSecrets;
        private readonly IMemoryCache _memoryCache;

        public JwtTokenMiddleware(
            IOptions<TechLanchesCognitoSecrets> cognitoOptions,
            IMemoryCache memoryCache)
        {
            _cognitoSecrets = cognitoOptions.Value;
            _memoryCache = memoryCache;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // Get the token from the Authorization header
            var token = await context.GetTokenAsync("access_token")
                ?? context.Request.Headers.Authorization.ToString().Replace("Bearer ", "");

            if (!token.IsNullOrEmpty())
            {
                //put token in cache
                _memoryCache.Set(Constants.AUTH_TOKEN_KEY, token, TimeSpan.FromMinutes(5));

                try
                {
                    var claimsPrincipal = ExtractClaimsFromJwt(token);

                    // Extract the user ID from the claims
                    var userIdClaim = claimsPrincipal.FindFirst("username");
                    var userId = userIdClaim?.Value;

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

        private static ClaimsPrincipal ExtractClaimsFromJwt(string token)
        {
            // Decode the token to extract claims
            var handler = new JwtSecurityTokenHandler();
            var decodedToken = handler.ReadJwtToken(token);

            // Create claims from the decoded token
            var claims = decodedToken.Claims.ToList();
            var identity = new ClaimsIdentity(claims, "jwt");

            // Return the claims principal
            return new ClaimsPrincipal(identity);
        }
    }
}