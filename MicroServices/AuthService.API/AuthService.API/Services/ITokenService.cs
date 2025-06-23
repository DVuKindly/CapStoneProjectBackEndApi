using AuthService.API.Entities;
using System.Security.Claims;

namespace AuthService.API.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(UserAuth user, List<string> permissionKeys);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
        bool IsRefreshTokenValid(string refreshToken);
        string GenerateIdToken(UserAuth user);
    }
}
