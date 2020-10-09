using ContentAggregator.Repositories.Hashes;
using ContentAggregator.Repositories.Users;
using ContentAggregator.Services.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace ContentAggregator.Web.Extensions
{
    public static class StartupExtensions
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IHashRepository, HashRepository>();
            services.AddScoped<IAuthService, AuthService>();
        }
    }
}
