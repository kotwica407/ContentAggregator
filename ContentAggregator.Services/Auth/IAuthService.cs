using System.Threading.Tasks;
using ContentAggregator.Models.Dtos;

namespace ContentAggregator.Services.Auth
{
    public interface IAuthService
    {
        Task LoginUserAsync(LoginDto dto);
        Task RegisterUserAsync(UserRegisterDto dto);
        Task RegisterAdminAsync(UserRegisterDto dto);
        Task Logout();
    }
}