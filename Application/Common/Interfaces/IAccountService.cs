using Application.Models;

namespace Application.Interfaces
{
    public interface IAccountService
    {
        Task<RequestResponse<JwtTokenResponse>> LoginAsync(LoginRequest login);
        Task<RequestResponse<JwtTokenResponse>> LoginAdminAsync(string userName, string passKey);
        Task<RequestResponse<JwtTokenResponse>> RegisterAsync(RegisterRequest register);
        Task<RequestResponse> ChangePasswordUserAsync(ChangePasswordRequest changePassword);
        Task<RequestResponse> ResetPasswordUserAsync(ResetPasswordRequest resetPassword);
        Task<RequestResponse> ValidateTokenAsync(string token);
        Task<bool> SendPassKeyAsync(string mobileNumber, string passKey);
        Task<bool> CheckPasswordAsync(int userId, string password);
    }
}