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
            var context = sp.GetService<UserContext>();

            context.Database.Migrate();
        }

        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEntityFrameworkNpgsql().AddDbContext<UserContext>(options =>
                options.UseNpgsql(configuration.GetSection("Database").GetValue<string>("ConnectionString")));

            services.EnsureMigrated();
        }
    }
}