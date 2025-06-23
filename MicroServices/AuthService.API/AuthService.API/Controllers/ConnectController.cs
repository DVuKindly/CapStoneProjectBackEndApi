using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AuthService.API.Controllers
{
    [ApiController]
    [Route("api/auth/connectinfo")]
    public class ConnectController : ControllerBase
    {
        [HttpGet("userinfo")]
        public IActionResult GetUserInfo()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null || !identity.IsAuthenticated)
                return Unauthorized();

            var claims = identity.Claims.ToDictionary(c => c.Type, c => c.Value);

            return Ok(new
            {
                // ✅ kiểm tra tồn tại hoặc fallback sang NameIdentifier nếu cần
                sub = claims.GetValueOrDefault(JwtRegisteredClaimNames.Sub)
                       ?? claims.GetValueOrDefault(ClaimTypes.NameIdentifier),
                name = claims.GetValueOrDefault(JwtRegisteredClaimNames.Name)
                       ?? claims.GetValueOrDefault(ClaimTypes.Name),
                email = claims.GetValueOrDefault(JwtRegisteredClaimNames.Email)
                        ?? claims.GetValueOrDefault(ClaimTypes.Email),
                roles = identity.FindAll(ClaimTypes.Role).Select(r => r.Value).ToArray()
            });
        }

    }

}
