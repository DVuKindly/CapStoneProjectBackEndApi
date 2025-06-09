using BffService.API.DTOs.AdminCreate;

using BffService.API.DTOs.Request;
using BffService.API.DTOs.Responses;

namespace BffService.API.Services
{
    public interface IAuthProxyService
    {
        // Người dùng tự đăng ký
        Task<AuthResponse?> RegisterAsync(RegisterRequest request);

        // SuperAdmin tạo Admin khu vực
        Task<AuthResponse?> RegisterAdminAsync(RegisterBySuperAdminRequest request, string token);

        // Admin tạo hệ thống account theo từng DTO cụ thể
        Task<AuthResponse?> RegisterManagerAsync(RegisterStaffRequest request, string token);
        Task<AuthResponse?> RegisterStaffAsync(RegisterStaffRequest request, string token);

        Task<AuthResponse?> RegisterCoachAsync(RegisterCoachRequest request, string token);
        Task<AuthResponse?> RegisterPartnerAsync(RegisterPartnerRequest request, string token);
        Task<AuthResponse?> RegisterSupplierAsync(RegisterSupplierRequest request, string token);

        // Đăng nhập / Token
        Task<AuthResponse?> LoginAsync(LoginRequest request);
        Task<AuthResponse?> GoogleLoginAsync(GoogleLoginRequest request);
        Task<AuthResponse?> RefreshTokenAsync(RefreshTokenRequest request);
        Task<AuthResponse?> LogoutAsync(string token);

        // Quên mật khẩu
        Task<AuthResponse?> ChangePasswordAsync(ChangePasswordRequest request, string token);
        Task<AuthResponse?> ForgotPasswordAsync(ForgotPasswordRequest request);
        Task<AuthResponse?> ResetPasswordAsync(ResetPasswordRequest request);

        // Xác minh Email
        Task<BaseResponse> VerifyEmailAsync(VerifyEmailRequest request);

        // Đặt mật khẩu sau khi được duyệt (Partner, Coach, Supplier...)
        Task<AuthResponse?> SetPasswordAsync(SetPasswordThirtyRequest request);

        // Kiểm tra trạng thái người dùng
        Task<AuthStatusResponse?> GetStatusAsync(string userId, string token);

        /// lấy danh sách khu vực 
        /// 
        Task<List<LocationDto>> GetAvailableLocationsAsync();
    }
}
