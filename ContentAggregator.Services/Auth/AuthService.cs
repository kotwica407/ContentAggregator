using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using ContentAggregator.Common;
using ContentAggregator.Common.Extensions;
using ContentAggregator.Models;
using ContentAggregator.Models.Dtos;
using ContentAggregator.Models.Exceptions;
using ContentAggregator.Models.Model;
using ContentAggregator.Repositories;
using ContentAggregator.Repositories.Hashes;
using ContentAggregator.Services.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ContentAggregator.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IHashRepository _hashRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;
        private readonly ICrudRepository<User> _userRepository;

        public AuthService(
            IHashRepository hashRepository,
            ICrudRepository<User> userRepository,
            IHttpContextAccessor httpContextAccessor,
            ILogger<AuthService> logger)
        {
            _hashRepository = hashRepository;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task Logout()
        {
            HttpContext context = _httpContextAccessor.HttpContext;
            if (!context.User.Identity.IsAuthenticated)
            {
                _logger.LogWarning("You cannot logout if you are not logged in");
                throw HttpError.Unauthorized("");
            }

            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task LoginUserAsync(LoginDto dto)
        {
            HttpContext context = _httpContextAccessor.HttpContext;
            if (context.User.Identity.IsAuthenticated)
                throw HttpError.Forbidden("User is still authenticated");

            if (dto.Name == null || dto.Password == null)
                throw HttpError.InternalServerError("Username or password is null");

            User user = (await _userRepository.Find(x => x.Name == dto.Name)).FirstOrDefault();
            if (user == null)
                throw HttpError.Unauthorized("Wrong username or password");

            string savedPasswordHash = (await _hashRepository.Get(user.Id))?.PasswordHash;

            if (savedPasswordHash == null)
                throw HttpError.InternalServerError("User has no password");

            if (!HashHelpers.CheckPasswordWithHash(dto.Password, savedPasswordHash))
                throw HttpError.Unauthorized("Wrong username or password");

            string[] roles = user.CredentialLevel.GetAllPossibleRoles();
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.NameIdentifier, user.Name)
            };
            claims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x)));

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.Now.AddDays(1),
                IsPersistent = dto.RememberMe
            };

            await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);
        }

        public Task RegisterUserAsync(UserRegisterDto dto) => RegisterUserAsync(dto, CredentialLevel.User);

        public Task RegisterAdminAsync(UserRegisterDto dto) => RegisterUserAsync(dto,
            CredentialLevel.User | CredentialLevel.Moderator | CredentialLevel.Admin);

        private static bool IsValidEmail(string emailAddress)
        {
            try
            {
                var m = new MailAddress(emailAddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private async Task RegisterUserAsync(UserRegisterDto dto, CredentialLevel credentialLevel)
        {
            #region Validate

            if (!IsValidEmail(dto.Email))
                throw HttpError.BadRequest("Invalid email address");

            if (dto.Name == null)
                throw HttpError.BadRequest("Username cannot be null");

            if (dto.Name.Length > Consts.UsernameMaxLength)
                throw HttpError.BadRequest("Username is too long");

            if (dto.Description != null && dto.Description.Length > Consts.DescriptionMaxLength)
                throw HttpError.BadRequest("Description is too long");

            #endregion

            #region CheckIfUserExists

            if ((await _userRepository.Find(x => x.Name == dto.Name)).Any())
                throw HttpError.BadRequest($"There is user with {dto.Name} username");

            if ((await _userRepository.Find(x => x.Email == dto.Email)).Any())
                throw HttpError.BadRequest($"There is user with {dto.Email} email address");

            #endregion

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                CredentialLevel = credentialLevel,
                Description = dto.Description,
                Email = dto.Email,
                Name = dto.Name
            };
            await _userRepository.Create(user);
            await _hashRepository.CreateOrUpdate(new Hash
            {
                Id = user.Id,
                PasswordHash = HashHelpers.CreateHash(dto.Password)
            });
        }
    }
}