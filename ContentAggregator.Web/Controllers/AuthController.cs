using ContentAggregator.Models.Dtos;
using ContentAggregator.Services.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ContentAggregator.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            bool loggedIn = await _authService.CheckPasswordAsync(dto);
            if (!loggedIn)
                return BadRequest("User is not logged in");
            return Ok("Logged in");
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            await _authService.RegisterUserAsync(dto);
            return Ok("User is registered");
        }
    }
}
