using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TechLanches.Adapter.AWS.SecretsManager;

namespace TechLanches.Adapter.SqlServer
{
    public static class DatabaseConfig
    {
        public static void AddDatabaseConfiguration(
            this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            if (services is null) throw new ArgumentNullException(nameof(services));

            var serviceProvider = services.BuildServiceProvider();

            var opt = serviceProvider.GetRequiredService<IOptions<TechLanchesPedidoDatabaseSecrets>>();            

            services.AddDbContext<TechLanchesDbContext>(config =>
            {
                config.UseSqlServer(GetConnectionString(opt.Value));
            },
            serviceLifetime);
        }

        public static string GetConnectionString(TechLanchesPedidoDatabaseSecrets opt)
        {
            return $"Server={opt.Host},{opt.Port};Database={opt.Database};User Id={opt.Username};Password={opt.Password};TrustServerCertificate=True;";
        }

        public static void UseDatabaseConfiguration(this IApplicationBuilder app)
        {
            if (app is null) throw new ArgumentNullException(nameof(app));

            SetDatabaseDefaults(app.ApplicationServices);
        }

        private static void SetDatabaseDefaults(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<TechLanchesDbContext>();

            context.Database.EnsureCreated();

            DataSeeder.Seed(context);
        }

    }
}
