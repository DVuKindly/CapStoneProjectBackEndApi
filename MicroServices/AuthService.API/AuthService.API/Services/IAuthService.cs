using AuthService.API.DTOs.AdminCreate;
using AuthService.API.DTOs.Request;
using AuthService.API.DTOs.Responses;

namespace AuthService.API.Services
{
    public interface IAuthService
    {
        // Người dùng tự đăng ký
        Task<AuthResponse> RegisterAsync(RegisterRequest request);

        // SuperAdmin tạo Admin khu vực
        Task<AuthResponse> RegisterAdminAsync(RegisterBySuperAdminRequest request);

        // Admin tạo các hệ thống account (Manager, Staff, Đối tác)
        Task<AuthResponse> RegisterManagerAsync(RegisterStaffRequest request);
        Task<AuthResponse> RegisterStaffAsync(RegisterStaffRequest request);
        Task<AuthResponse> RegisterCoachAsync(RegisterCoachRequest request);
        Task<AuthResponse> RegisterPartnerAsync(RegisterPartnerRequest request);
        Task<AuthResponse> RegisterSupplierAsync(RegisterSupplierRequest request);

        // Đăng nhập / Token
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> GoogleLoginAsync(GoogleLoginRequest request);
        Task<AuthResponse> RefreshTokenAsync(string token);
        Task<AuthResponse> LogoutAsync(string token);

        // Quên mật khẩu
        Task<AuthResponse> ChangePasswordAsync(ChangePasswordRequest request, string token);
        Task<AuthResponse> ForgotPasswordAsync(ForgotPasswordRequest request);
        Task<AuthResponse> ResetPasswordAsync(ResetPasswordRequest request);

        // Xác minh Email
        Task<bool> VerifyEmailAsync(string token);

        // Đặt mật khẩu sau khi được duyệt (Partner, Coach, Supplier...)
        Task<AuthResponse> SetPasswordAsync(SetPasswordThirtyRequest request);

        // Kiểm tra trạng thái người dùng
        Task<AuthStatusResponse> GetStatusAsync(string userId);
    }
}
