using AuthService.API.DTOs.Request;
using AuthService.API.DTOs.Responses;
using AuthService.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;

        public AuthController(IAuthService authService, ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;  
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
        public async Task<IActionResult> Logout()
        {
          
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

          
            var userPrincipal = _tokenService.GetPrincipalFromExpiredToken(token);
            if (userPrincipal == null)
            {
                return Unauthorized(new BaseResponse { Success = false, Message = "Invalid token." });
            }

            var userIdClaim = userPrincipal.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized(new BaseResponse { Success = false, Message = "User ID not found in token." });
            }

            var userId = userIdClaim.Value;

           
            await _authService.LogoutAsync(userId);

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
            var result = await _authService.ForgotPasswordAsync(request);  
            return result.Success
                ? Ok(new BaseResponse { Success = true, Message = "Reset link sent to email." })
                : BadRequest(new BaseResponse { Success = false, Message = result.Message });
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var result = await _authService.ResetPasswordAsync(request);  
            if (result.Success)
            {
                return Ok(new BaseResponse { Success = true, Message = "Password reset successful." });
            }
            else
            {
                return BadRequest(new BaseResponse { Success = false, Message = result.Message });
            }
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

            if (!result.Success)
            {
                return BadRequest(new BaseResponse { Success = false, Message = result.Message });
            }

            return Ok(new BaseResponse { Success = true, Message = result.Message });
        }



        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request)
        {
            var result = await _authService.VerifyEmailAsync(request.Token);
            return result
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
