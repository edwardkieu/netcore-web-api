﻿using Application.DTOs.Account;
using Application.Wrappers;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAccountService
    {
        Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress);

        Task<Response<string>> RegisterAsync(RegisterRequest request, string origin);

        Task<Response<string>> ConfirmEmailAsync(string userId, string code);

        Task ForgotPassword(ForgotPasswordRequest model, string origin);

        Task<Response<string>> ResetPassword(ResetPasswordRequest model);

        Task<string> GetUserName(string userId);

        Task<Response<UserContextDto>> GetUserAsync(string userId);

        Task<Response<AuthenticationResponse>> VerifyAndGenerateToken(RefreshTokenRequest tokenRequest);

        Task<bool> RevokeToken(string token);
    }
}