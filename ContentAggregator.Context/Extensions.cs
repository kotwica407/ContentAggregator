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
            var dbSection = configuration.GetSection("Database");
            if (dbSection.GetValue<string>("Provider") == "Postgres")
            {
                services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(configuration.GetSection("Database").GetValue<string>("ConnectionString")));
                services.EnsureMigrated();
            }
            else if (dbSection.GetValue<string>("Provider") == "InMemory")
            {
                services.AddEntityFrameworkInMemoryDatabase()
                   .AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}