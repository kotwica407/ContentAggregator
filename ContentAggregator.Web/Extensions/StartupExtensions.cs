using ContentAggregator.Repositories.Hashes;
using ContentAggregator.Repositories.Posts;
using ContentAggregator.Repositories.Tags;
using ContentAggregator.Repositories.Users;
using ContentAggregator.Services.Auth;
using ContentAggregator.Services.Posts;
using Microsoft.Extensions.DependencyInjection;

namespace ContentAggregator.Web.Extensions
{
    public static class StartupExtensions
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IHashRepository, HashRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IPostRepository, PostRepository>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPostService, PostService>();

            services.AddHttpContextAccessor();
        }
    }
}
