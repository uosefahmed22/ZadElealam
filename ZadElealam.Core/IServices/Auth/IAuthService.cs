using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZadElealam.Core.Errors;
using ZadElealam.Core.Models.Auth;

namespace ZadElealam.Core.IServices.Auth
{
    public interface IAuthService
    {
        Task<ApiResponse> RigsterAsync(Register model, Func<string, string, string> generateCallBackUrl);
        Task<ApiResponse> LoginAsync(Login model);
        Task<ApiResponse> RefreshToken([FromBody] TokenRequest model);
        Task<ApiResponse> RevokeToken([FromBody] TokenRequest model);
        Task<ApiResponse> ForgetPassword(string email);
        ApiResponse VerfiyOtp(VerifyOtp model);
        Task<ApiResponse> ResetPasswordAsync(ResetPassword model);
        Task<ApiResponse> ResendConfirmationEmailAsync(string email, Func<string, string, string> generateCallBackUrl);
        Task<bool> ConfirmUserEmailAsync(string userId, string token);
        Task<ApiResponse> ChangePasswordAsync(ChangePassword model, string email);
    }
}
