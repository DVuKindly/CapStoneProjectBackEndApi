using AuthService.API.DTOs.Request;
using AuthService.API.DTOs.Responses;

namespace AuthService.API.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task LogoutAsync(string userId);
        Task<AuthResponse> RefreshTokenAsync(string token);
        Task<bool> ForgotPasswordAsync(ForgotPasswordRequest request);
        Task<bool> ResetPasswordAsync(ResetPasswordRequest request);
        Task<bool> ChangePasswordAsync(ChangePasswordRequest request);
        Task<bool> VerifyEmailAsync(string token);
        Task<AuthStatusResponse> GetStatusAsync(string userId);
        Task<AuthResponse> GoogleLoginAsync(GoogleLoginRequest request);
    }
}
