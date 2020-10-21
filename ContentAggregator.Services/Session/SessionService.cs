using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ContentAggregator.Models.Model;
using ContentAggregator.Repositories.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ContentAggregator.Services.Session
{
    public class SessionService : ISessionService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;

        public SessionService(
            ILogger<SessionService> logger,
            IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        public async Task<User> GetUser()
        {
            if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                return null;

            string userName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            return (await _userRepository.Find(x => x.Name == userName)).SingleOrDefault();
        }
    }
}