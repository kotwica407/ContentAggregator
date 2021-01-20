using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

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

            string dbProvider = configuration.GetSection("Database").GetValue<string>("Provider");

            switch (dbProvider)
            {
                case "MSSQL":
                    //services.AddEntityFrameworkSqlServer().AddDbContext<ApplicationDbContext>(options => 
                    //    options.UseSqlServer(configuration.GetSection("Database").GetValue<string>("ConnectionString")));
                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseSqlServer(configuration.GetSection("Database").GetValue<string>("ConnectionString")));
                    break;
                case "Postgres":
                    services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationDbContext>(options =>
                        options.UseNpgsql(configuration.GetSection("Database").GetValue<string>("ConnectionString")));
                    break;
                default:
                    throw new NotSupportedException("Provider put in configuration is not supported");
            }

            //services.EnsureMigrated();
        }
    }
}