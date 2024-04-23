using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using TechLanches.Adapter.API.Health;
using TechLanches.Adapter.AWS.SecretsManager;
using TechLanches.Adapter.SqlServer;

namespace TechLanches.Adapter.API.Configuration
{
    public static class HealthCheckConfig
    {
        public static void AddHealthCheckConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var serviceProvider = services.BuildServiceProvider();

            var opt = serviceProvider.GetRequiredService<IOptions<TechLanchesDatabaseSecrets>>();

            services.AddHealthChecks()
                .AddSqlServer(connectionString: DatabaseConfig.GetConnectionString(opt.Value), name: "Banco de dados Tech Lanches")
                .AddCheck<RabbitMQHealthCheck>("rabbit_hc");
        }

        public static void AddHealthCheckEndpoint(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health", new HealthCheckOptions
                {
                    Predicate = _ => true,
                });
            });
        }
    }
}
