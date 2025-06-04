using AuthService.API.DTOs.Request;
using AuthService.API.DTOs.Responses;

namespace AuthService.API.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> RefreshTokenAsync(string token);
        Task<AuthResponse> ChangePasswordAsync(ChangePasswordRequest request, string token);
        Task<AuthResponse> ForgotPasswordAsync(ForgotPasswordRequest request);
        Task<AuthResponse> ResetPasswordAsync(ResetPasswordRequest request);
        Task<AuthResponse> GoogleLoginAsync(GoogleLoginRequest request);
        Task<AuthResponse> LogoutAsync(string token);
        Task<bool> VerifyEmailAsync(string token);
        Task<AuthStatusResponse> GetStatusAsync(string userId);
    }
}
