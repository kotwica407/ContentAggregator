using ContentAggregator.Models.Dtos;
using ContentAggregator.Services.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
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
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.Now.AddDays(1),
                IsPersistent = dto.RememberMe
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);
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
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok("");
        }
    }
}
