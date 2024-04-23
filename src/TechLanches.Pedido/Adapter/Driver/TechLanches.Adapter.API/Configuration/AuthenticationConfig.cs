using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using TechLanches.Adapter.API.Options;
using TechLanches.Adapter.AWS.SecretsManager;

namespace TechLanches.Adapter.API.Configuration
{
    public static class AuthenticationConfig
    {
        public static void AddAuthenticationConfig(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var opt = serviceProvider.GetRequiredService<IOptions<TechLanchesCognitoSecrets>>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = opt.Value.CognitoUri;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = opt.Value.CognitoUri,
                    ValidateLifetime = true,
                    LifetimeValidator = (before, expires, token, param) => expires > DateTime.UtcNow,
                    ValidateAudience = false,
                };
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.HandleResponse(); 
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";

                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                        {
                            StatusCode = StatusCodes.Status401Unauthorized,
                            Message = "Unauthorized"
                        }));
                    }
                };
            }
            );
            services.AddAuthorization();
        }
    }
}
