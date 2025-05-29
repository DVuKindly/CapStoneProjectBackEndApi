using AuthService.API.DTOs.Request;
using AuthService.API.DTOs.Responses;
using AuthService.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _authService.RegisterAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);
            return result.Success ? Ok(result) : Unauthorized(result);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            await _authService.LogoutAsync(request.UserId);
            return Ok(new BaseResponse { Success = true, Message = "Logged out successfully." });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var result = await _authService.RefreshTokenAsync(request.RefreshToken);
            return result.Success ? Ok(result) : Unauthorized(result);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var success = await _authService.ForgotPasswordAsync(request);
            return success
                ? Ok(new BaseResponse { Success = true, Message = "Reset link sent to email." })
                : NotFound(new BaseResponse { Success = false, Message = "Email not found." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var success = await _authService.ResetPasswordAsync(request);
            return success
                ? Ok(new BaseResponse { Success = true, Message = "Password reset successful." })
                : BadRequest(new BaseResponse { Success = false, Message = "Invalid or expired token." });
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var success = await _authService.ChangePasswordAsync(request);
            return success
                ? Ok(new BaseResponse { Success = true, Message = "Password changed successfully." })
                : BadRequest(new BaseResponse { Success = false, Message = "Invalid old password." });
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request)
        {
            var success = await _authService.VerifyEmailAsync(request.Token);
            return success
                ? Ok(new BaseResponse { Success = true, Message = "Email verified successfully." })
                : BadRequest(new BaseResponse { Success = false, Message = "Invalid or expired token." });
        }

        [HttpGet("status/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetStatus([FromRoute] string userId)
        {
            var status = await _authService.GetStatusAsync(userId);
            return Ok(status);
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
        {
            var result = await _authService.GoogleLoginAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
