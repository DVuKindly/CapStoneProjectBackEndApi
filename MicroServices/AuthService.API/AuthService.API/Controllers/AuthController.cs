using AuthService.API.DTOs;
using AuthService.API.DTOs.Request;
using AuthService.API.DTOs.Responses;
using AuthService.API.Middlewares;
using AuthService.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AuthService.API.Repositories;
using AuthService.API.Extensions;

namespace AuthService.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly IAccountQueryService _accountQueryService;
        private readonly IUserServiceClient _userServiceClient;

        public AuthController(IAuthService authService, ITokenService tokenService, IUserRepository userRepository, IAccountQueryService accountQueryService, IUserServiceClient userServiceClient)
        {
            _authService = authService;
            _tokenService = tokenService;
            _userRepository = userRepository;
            _accountQueryService = accountQueryService;
            _userServiceClient = userServiceClient;
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
            var userIdClaim = userPrincipal?.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized(new BaseResponse { Success = false, Message = "Invalid token." });

            await _authService.LogoutAsync(userIdClaim.Value);
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
            return result.Success ? Ok(new BaseResponse { Success = true, Message = "Reset link sent to email." }) : BadRequest(new BaseResponse { Success = false, Message = result.Message });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var result = await _authService.ResetPasswordAsync(request);
            return result.Success ? Ok(new BaseResponse { Success = true, Message = "Password reset successful." }) : BadRequest(new BaseResponse { Success = false, Message = result.Message });
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token)) return Unauthorized(new BaseResponse { Success = false, Message = "Token is missing or invalid." });

            var result = await _authService.ChangePasswordAsync(request, token);
            return result.Success ? Ok(new BaseResponse { Success = true, Message = result.Message }) : BadRequest(new BaseResponse { Success = false, Message = result.Message });
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request)
        {
            var result = await _authService.VerifyEmailAsync(request.Token);
            return result ? Ok(new BaseResponse { Success = true, Message = "Email verified successfully." }) : BadRequest(new BaseResponse { Success = false, Message = "Invalid or expired token." });
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
        {
            var result = await _authService.GoogleLoginAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("set-passwordthirtytoken")]
        [AllowAnonymous]
        public async Task<IActionResult> SetPassword([FromBody] SetPasswordThirtyRequest request)
        {
            var response = await _authService.SetPasswordAsync(request);
            return Ok(response);
        }

        [HttpGet("status/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetStatus([FromRoute] string userId)
        {
            var status = await _authService.GetStatusAsync(userId);
            return Ok(status);
        }

        [HttpGet("locationsAvaible")]
        public async Task<IActionResult> GetAvailableLocations()
        {
            var locations = await _authService.GetLocationsAsync();
            return Ok(locations);
        }

        [HttpGet("accounts")]
        [Authorize(Roles = "super_admin,admin,manager,staff_service,staff_onboarding,staff_content")]
        public async Task<IActionResult> GetAccounts([FromQuery] string[] roleKeys)
        {
            var currentUserId = User.GetAccountId();

            // Gọi AuthService để xử lý logic phân quyền + gọi đến AccountQueryService bên trong
            var result = await _authService.GetFilteredAccountsByCurrentUserAsync(currentUserId, roleKeys);
            return Ok(result);
        }



    }

    public class PromoteToMemberRequest
    {
        public Guid AccountId { get; set; }
    }
}
