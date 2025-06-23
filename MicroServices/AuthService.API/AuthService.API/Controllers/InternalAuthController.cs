using AuthService.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AuthService.API.Controllers
{
    [ApiController]
    [Route("api/auth/internal")]

    public class InternalAuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<InternalAuthController> _logger;
        private readonly string _validApiKey;

        public InternalAuthController(IAuthService authService, ILogger<InternalAuthController> logger, IConfiguration config)
        {
            _authService = authService;
            _logger = logger;
            _validApiKey = config.GetValue<string>("InternalApi:ApiKey");

            if (string.IsNullOrEmpty(_validApiKey))
                _logger.LogWarning("API key for internal auth is not configured!");
        }

        private bool ValidateApiKey()
        {
            if (!Request.Headers.TryGetValue("X-Api-Key", out var extractedApiKey))
            {
                _logger.LogWarning("API key header missing");
                return false;
            }

            if (string.IsNullOrEmpty(extractedApiKey))
            {
                _logger.LogWarning("API key header empty");
                return false;
            }

            if (!_validApiKey.Equals(extractedApiKey))
            {
                _logger.LogWarning("API key does not match");
                return false;
            }

            return true;
        }

        [HttpPost("promote-to-member")]
        public async Task<IActionResult> PromoteToMemberInternal([FromBody] PromoteToMemberRequest request)
        {
            var apiKeyHeader = Request.Headers["X-Api-Key"].ToString();
            _logger.LogInformation($"[InternalAuthController] Received API key: {apiKeyHeader}");

            if (!ValidateApiKey())
            {
                _logger.LogWarning($"[InternalAuthController] Invalid API key attempt: {apiKeyHeader}");
                return Unauthorized(new { Message = "Unauthorized" });
            }

            if (request == null || request.AccountId == Guid.Empty)
            {
                _logger.LogWarning("[InternalAuthController] Invalid AccountId");
                return BadRequest(new { Message = "Invalid AccountId" });
            }

            try
            {
                var success = await _authService.PromoteUserToMemberAsync(request.AccountId);
                if (!success)
                {
                    _logger.LogError($"[InternalAuthController] Failed to promote user {request.AccountId} to member.");
                    return BadRequest(new { Message = "Could not update role" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[InternalAuthController] Exception occurred while promoting user.");
                return StatusCode(500, new { Message = "Internal server error" });
            }

            return Ok(new { Message = "Role updated successfully" });
        }
    }
}
