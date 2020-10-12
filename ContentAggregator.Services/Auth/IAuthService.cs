using System.Security.Claims;
using System.Threading.Tasks;
using ContentAggregator.Models.Dtos;

namespace ContentAggregator.Services.Auth
{
    public interface IAuthService
    {
        Task<bool> CheckPasswordAsync(LoginDto dto);
        Task<ClaimsPrincipal> LoginUserAsync(LoginDto dto);
        Task RegisterUserAsync(UserRegisterDto dto);
    }
}