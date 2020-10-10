﻿using ContentAggregator.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ContentAggregator.Services.Auth
{
    public interface IAuthService
    {
        Task<bool> CheckPasswordAsync(LoginDto dto);
        Task<ClaimsPrincipal> LoginUserAsync(LoginDto dto);
        Task RegisterUserAsync(UserRegisterDto dto);
    }
}
