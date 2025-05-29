
using AuthService.API.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace AuthService.API.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _jwtSecret;
        private readonly TimeSpan _accessTokenExpiration = TimeSpan.FromMinutes(30);
        private readonly TimeSpan _refreshTokenExpiration = TimeSpan.FromDays(7);

        public TokenService()
        {
            _jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")
                         ?? throw new InvalidOperationException("JWT_SECRET_KEY is missing in environment variables or .env.");
        }

        public string GenerateAccessToken(UserAuth user)
        {
            var roles = user.UserRoles?.Select(r => r.Role.RoleKey).ToList() ?? new();

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim("roles", JsonConvert.SerializeObject(roles))
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "AuthService",
                audience: "AllMicroservices",
                claims: claims,
                expires: DateTime.UtcNow.Add(_accessTokenExpiration),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);

                if (validatedToken is JwtSecurityToken jwt &&
                    jwt.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return principal;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public bool IsRefreshTokenValid(string refreshToken)
        {
            return !string.IsNullOrWhiteSpace(refreshToken) && refreshToken.Length > 32;
        }
    }
}