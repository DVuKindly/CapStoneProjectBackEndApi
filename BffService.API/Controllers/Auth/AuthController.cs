using BffService.API.DTOs.AdminCreate;
using BffService.API.DTOs.Request;
using BffService.API.DTOs.Responses;
using BffService.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BffService.API.Controllers
{
    [Route("api/bff/auth")]
    [ApiController]
    public class AuthProxyController : ControllerBase
    {
        private readonly IAuthProxyService _authService;

        public AuthProxyController(IAuthProxyService authService)
        {
            _authService = authService;
        }

        // 🔐 Auth - Register/Login/Token
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request) =>
            await HandleResult(() => _authService.RegisterAsync(request));

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request) =>
            await HandleResult(() => _authService.LoginAsync(request));

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request) =>
            await HandleResult(() => _authService.RefreshTokenAsync(request));

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var token = GetAccessToken();
            return await HandleResult(() => _authService.LogoutAsync(token));
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request) =>
            await HandleResult(() => _authService.GoogleLoginAsync(request));

        // 📧 Email Verification & Password
        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request) =>
            await HandleResult(() => _authService.VerifyEmailAsync(request));

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request) =>
            await HandleResult(() => _authService.ForgotPasswordAsync(request));

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request) =>
            await HandleResult(() => _authService.ResetPasswordAsync(request));

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var token = GetAccessToken();
            return await HandleResult(() => _authService.ChangePasswordAsync(request, token));
        }

        // 📊 Status
        [HttpGet("status/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetStatus(string userId)
        {
            var token = GetAccessToken();
            return await HandleResult(() => _authService.GetStatusAsync(userId, token));
        }

        // 🧑‍💼 System Account Creation (SuperAdmin/Admin)
        [HttpPost("superadmin-register/admin")]
        [Authorize(Roles = "super_admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterBySuperAdminRequest request) =>
            await HandleResult(() => _authService.RegisterAdminAsync(request, GetAccessToken()));

        [HttpPost("admin-register/manager")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> RegisterManager([FromBody] RegisterStaffRequest request) =>
            await HandleResult(() => _authService.RegisterManagerAsync(request, GetAccessToken()));

        [HttpPost("admin-register/staff-onboarding")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> RegisterStaffOnboarding([FromBody] RegisterStaffRequest request)
        {
            request.RoleKey = "staff_onboarding";
            return await HandleResult(() => _authService.RegisterStaffAsync(request, GetAccessToken()));
        }

        [HttpPost("admin-register/staff-service")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> RegisterStaffService([FromBody] RegisterStaffRequest request)
        {
            request.RoleKey = "staff_service";
            return await HandleResult(() => _authService.RegisterStaffAsync(request, GetAccessToken()));
        }

        [HttpPost("admin-register/staff-content")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> RegisterStaffContent([FromBody] RegisterStaffRequest request)
        {
            request.RoleKey = "staff_content";
            return await HandleResult(() => _authService.RegisterStaffAsync(request, GetAccessToken()));
        }

        [HttpPost("admin-register/coach")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> RegisterCoach([FromBody] RegisterCoachRequest request) =>
            await HandleResult(() => _authService.RegisterCoachAsync(request, GetAccessToken()));

        [HttpPost("admin-register/partner")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> RegisterPartner([FromBody] RegisterPartnerRequest request) =>
            await HandleResult(() => _authService.RegisterPartnerAsync(request, GetAccessToken()));

        [HttpPost("admin-register/supplier")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> RegisterSupplier([FromBody] RegisterSupplierRequest request) =>
            await HandleResult(() => _authService.RegisterSupplierAsync(request, GetAccessToken()));

        [HttpPost("set-passwordthirtytoken")]
        [AllowAnonymous]
        public async Task<IActionResult> SetPassword([FromBody] SetPasswordThirtyRequest request) =>
            await HandleResult(() => _authService.SetPasswordAsync(request));

        // 📦 Helpers
        private string GetAccessToken()
        {
            return Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        }

        private async Task<IActionResult> HandleResult<T>(Func<Task<T?>> func)
        {
            var result = await func();

            if (result is null)
                return StatusCode(500, new { Success = false, Message = "Null response from AuthService." });

            if (result is BaseResponse baseResult)
                return baseResult.Success ? Ok(baseResult) : BadRequest(baseResult);

            return Ok(result);
        }

    }
}
