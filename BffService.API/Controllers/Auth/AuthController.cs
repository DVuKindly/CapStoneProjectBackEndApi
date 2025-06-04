using BffService.API.DTOs.Auth.Request;
using BffService.API.DTOs.Auth.Response;
using BffService.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BffService.API.Controllers.Auth
{
    [Route("api/bff/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthProxyService _authService;

        public AuthController(AuthProxyService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _authService.RegisterAsync(request);
            return result?.Success == true ? Ok(result) : BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);
            return result?.Success == true ? Ok(result) : Unauthorized(result);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            var result = await _authService.RefreshTokenAsync(request);
            return result?.Success == true ? Ok(result) : Unauthorized(result);
        }

        [HttpGet("verify-email")]
        public async Task<IActionResult> RedirectVerifyEmail([FromQuery] string token)
        {
            var result = await _authService.VerifyEmailAsync(new VerifyEmailRequest { Token = token });
            if (result?.Success == true)
                return Ok("✅ Email verified successfully!");
            return BadRequest("❌ Verification failed or token expired.");
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var result = await _authService.ForgotPasswordAsync(request);
            return result?.Success == true ? Ok(result) : BadRequest(result);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var result = await _authService.ResetPasswordAsync(request);
            return result?.Success == true ? Ok(result) : BadRequest(result);
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

          
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new BaseResponse { Success = false, Message = "Token is missing or invalid." });
            }

            var result = await _authService.ChangePasswordAsync(request, token);
            return result?.Success == true ? Ok(result) : BadRequest(result);
        }




        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
        {
            var result = await _authService.GoogleLoginAsync(request);
            return result?.Success == true ? Ok(result) : BadRequest(result);
        }

        [HttpGet("status/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetStatus([FromRoute] string userId)
        {
            var token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            var result = await _authService.GetStatusAsync(userId, token);
            return Ok(result);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var result = await _authService.LogoutAsync(token);
            return result?.Success == true ? Ok(result) : Unauthorized(result);
        }



    }
}