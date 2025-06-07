using System.Security.Claims;

namespace AuthService.API.Helpers
{
    public static class UserContextHelper
    {
        public static string? GetUserLocation(ClaimsPrincipal user) =>
            user.FindFirst("location")?.Value;

        public static string? GetUserId(ClaimsPrincipal user) =>
            user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public static string? GetUserRole(ClaimsPrincipal user) =>
            user.FindFirst(ClaimTypes.Role)?.Value;
    }
}
