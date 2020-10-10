using ContentAggregator.Common;
using ContentAggregator.Common.Extensions;
using ContentAggregator.Context.Entities;
using ContentAggregator.Models.Dtos;
using ContentAggregator.Repositories.Hashes;
using ContentAggregator.Repositories.Users;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ContentAggregator.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IHashRepository _hashRepository;
        private readonly IUserRepository _userRepository;
        public AuthService(IHashRepository hashRepository, IUserRepository userRepository)
        {
            _hashRepository = hashRepository;
            _userRepository = userRepository;
        }
        public async Task<bool> CheckPasswordAsync(LoginDto dto)
        {
            if (dto.Name == null || dto.Password == null)
                return false;

            var user = await _userRepository.GetByUserName(dto.Name);
            if(user == null)
            {
                return false;
            }
            string savedPasswordHash = (await _hashRepository.Get(user.Id))?.PasswordHash;
            if(savedPasswordHash == null)
            {
                throw new KeyNotFoundException();
            }
            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            var pbkdf2 = new Rfc2898DeriveBytes(dto.Password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    return false;

            return true;
        }

        public async Task<ClaimsPrincipal> LoginUserAsync(LoginDto dto)
        {
            if (dto.Name == null || dto.Password == null)
                throw new Exception("Username or password is null");

            var user = await _userRepository.GetByUserName(dto.Name);
            if (user == null)
            {
                throw new Exception("Wrong username or password");
            }
            string savedPasswordHash = (await _hashRepository.Get(user.Id))?.PasswordHash;
            if (savedPasswordHash == null)
            {
                throw new Exception("User has no password");
            }
            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            var pbkdf2 = new Rfc2898DeriveBytes(dto.Password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    throw new Exception("Wrong username or password");
            var roles = user.CredentialLevel.GetAllPossibleRoles();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.NameIdentifier, user.Name)
            };
            claims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x)));
            
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            return new ClaimsPrincipal(identity);
        }

        public async Task RegisterUserAsync(UserRegisterDto dto)
        {
            #region Validate
            if (!IsValidEmail(dto.Email))
                throw new Exception("Invalid email address");

            if (dto.Name == null)
                throw new Exception("Username cannot be null");

            if (dto.Name.Length > Consts.UsernameMaxLength)
                throw new Exception("Username is too long");

            if (dto.Description != null && dto.Description.Length > Consts.DescriptionMaxLength)
                throw new Exception("Description is too long");
            #endregion
            #region CheckIfUserExists
            if ((await _userRepository.GetByUserName(dto.Name)) != null)
                throw new Exception($"There is user with {dto.Name} username");

            if ((await _userRepository.GetByEmail(dto.Email)) != null)
                throw new Exception($"There is user with {dto.Email} email address");
            #endregion

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                CredentialLevel = Models.CredentialLevel.User,
                Description = dto.Description,
                Email = dto.Email,
                Name = dto.Name
            };
            await _userRepository.Create(user);
            await _hashRepository.CreateOrUpdate(new HashEntity
            {
                UserId = user.Id,
                PasswordHash = CreateHash(dto.Password)
            });
        }

        private string CreateHash(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            return Convert.ToBase64String(hashBytes);
        }

        private bool IsValidEmail(string emailAddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailAddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
