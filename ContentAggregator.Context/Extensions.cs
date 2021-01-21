using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ContentAggregator.Context
{
    public static class Extensions
    {
        private static void EnsureMigrated(this IServiceCollection services)
        {
            ServiceProvider sp = services.BuildServiceProvider();
            var context = sp.GetService<ApplicationDbContext>();

            context.Database.Migrate();
        }

        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            string dbProvider = configuration.GetValue<string>("DbProvider");

            switch (dbProvider)
            {
                case "MSSQL":
                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseSqlServer(configuration.GetConnectionString("ApplicationDbContext")));
                    services.EnsureMigrated();
                    break;
                case "Postgres":
                    services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationDbContext>(options =>
                        options.UseNpgsql(configuration.GetConnectionString("ApplicationDbContext")));
                    services.EnsureMigrated();
                    break;
                case "InMemory":
                    services.AddEntityFrameworkInMemoryDatabase().AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDb");
                    });
                    break;
                default:
                    throw new NotSupportedException("Provider put in configuration is not supported");
            }
        }
    }
}