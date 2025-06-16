using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AuthService.API.Controllers
{
    [ApiController]
    [Route("connect")]
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
                sub = claims[JwtRegisteredClaimNames.Sub],
                name = claims.GetValueOrDefault(JwtRegisteredClaimNames.Name),
                email = claims.GetValueOrDefault(JwtRegisteredClaimNames.Email),
                roles = identity.FindAll(ClaimTypes.Role).Select(r => r.Value).ToArray()
            });
        }
    }

}
