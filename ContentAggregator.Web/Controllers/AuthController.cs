using ContentAggregator.Models.Dtos;
using ContentAggregator.Services.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ContentAggregator.Web.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
                return BadRequest("User is still authenticated");
            var principal = await _authService.LoginUserAsync(dto);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            return Ok("Logged in");
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            await _authService.RegisterUserAsync(dto);
            return Ok("User is registered");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
                return Unauthorized("");
            await HttpContext.SignOutAsync();
            return Ok("");
        }
    }
}
