using AuthService.API.DTOs.Request;
using AuthService.API.DTOs.Responses;
using AuthService.API.Helpers;
using AuthService.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AuthService.API.Controllers
{
    [ApiController]
    [Route("connect")]
    public class TokenController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly JwtSettings _jwtSettings;

        public TokenController(IAuthService authService, IOptions<JwtSettings> jwtOptions)
        {
            _authService = authService;
            _jwtSettings = jwtOptions.Value;
        }

        [HttpPost("token")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> Token([FromForm] TokenRequestForm form)
        {
            // Thiết lập mặc định nếu không truyền từ FE
            var scope = string.IsNullOrWhiteSpace(form.Scope) ? "openid profile" : form.Scope;
            var clientId = string.IsNullOrWhiteSpace(form.ClientId) ? "default-client" : form.ClientId;

            Console.WriteLine($"[Token Request] grant_type={form.GrantType}, scope={scope}, client_id={clientId}");

            OidcTokenResponse response;

            if (form.GrantType == "password")
            {
                if (string.IsNullOrEmpty(form.Username) || string.IsNullOrEmpty(form.Password))
                {
                    return BadRequest(new
                    {
                        error = "invalid_request",
                        error_description = "Missing username or password."
                    });
                }

                var result = await _authService.LoginAsync(new LoginRequest
                {
                    Email = form.Username,
                    Password = form.Password
                });

                if (!result.Success)
                {
                    return Unauthorized(new
                    {
                        error = "invalid_grant",
                        error_description = result.Message
                    });
                }

                response = new OidcTokenResponse
                {
                    AccessToken = result.AccessToken,
                    RefreshToken = result.RefreshToken,
                    IdToken = result.IdToken,
                    ExpiresIn = _jwtSettings.AccessTokenMinutes * 60,
                    Email = result.Email,
                    FullName = result.FullName,
                    Role = result.Role ?? string.Empty,
                    UserId = result.UserId,
                    LocationId = result.LocationId
                };
            }
            else if (form.GrantType == "refresh_token")
            {
                if (string.IsNullOrEmpty(form.RefreshToken))
                {
                    return BadRequest(new
                    {
                        error = "invalid_request",
                        error_description = "Missing refresh_token."
                    });
                }

                var result = await _authService.RefreshTokenAsync(form.RefreshToken);

                if (!result.Success)
                {
                    return Unauthorized(new
                    {
                        error = "invalid_grant",
                        error_description = result.Message
                    });
                }

                response = new OidcTokenResponse
                {
                    AccessToken = result.AccessToken,
                    RefreshToken = result.RefreshToken,
                    IdToken = result.IdToken,
                    ExpiresIn = _jwtSettings.AccessTokenMinutes * 60,
                    Email = result.Email,
                    FullName = result.FullName,
                    Role = result.Role ?? string.Empty,
                    UserId = result.UserId,
                    LocationId = result.LocationId
                };
            }
            else
            {
                return BadRequest(new
                {
                    error = "unsupported_grant_type",
                    error_description = "Only 'password' and 'refresh_token' are supported."
                });
            }

            return Ok(response);
        }
    }
}
