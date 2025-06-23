using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthService.API.Helpers;
using Microsoft.Extensions.Options;

namespace AuthService.API.Controllers
{
    [ApiController]
    [Route("api/auth/oidc")]

    public class OidcController : ControllerBase
    {
        private readonly JwtSettings _jwtSettings;

        public OidcController(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        [HttpGet("openid-configuration")]
        public IActionResult GetOpenIdConfiguration()
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host.Value}";

            return Ok(new
            {
                issuer = _jwtSettings.Issuer,
                authorization_endpoint = $"{baseUrl}/api/auth/login",
                token_endpoint = $"{baseUrl}/api/auth/token",
                userinfo_endpoint = $"{baseUrl}/connect/userinfo",
                jwks_uri = $"{baseUrl}/.well-known/jwks.json",
                response_types_supported = new[] { "code", "token", "id_token" },
                subject_types_supported = new[] { "public" },
                id_token_signing_alg_values_supported = new[] { "RS256", "HS256" }
            });
        }

        [HttpGet("jwks.json")]
        public IActionResult GetJwks()
        {
            // TODO: Nếu dùng asymmetric (RSA) thì return public key
            return Ok(new { keys = new object[] { } }); // dùng symmetric thì bỏ trống
        }
    }

}

