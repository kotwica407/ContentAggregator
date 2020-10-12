﻿using System.Threading.Tasks;
using ContentAggregator.Models.Dtos;

namespace ContentAggregator.Services.Auth
{
    public interface IAuthService
    {
        Task<bool> CheckPasswordAsync(LoginDto dto);
        Task LoginUserAsync(LoginDto dto);
        Task RegisterUserAsync(UserRegisterDto dto);
        Task Logout();
    }
}