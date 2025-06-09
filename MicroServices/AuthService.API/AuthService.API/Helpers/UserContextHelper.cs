using System.Security.Claims;

namespace AuthService.API.Helpers
{
    public static class UserContextHelper
    {
        public static Guid? GetUserLocationId(ClaimsPrincipal user)
        {
            var locationStr = user.FindFirst("location")?.Value;
            return Guid.TryParse(locationStr, out var locationId) ? locationId : null;
        }

        public static Guid? GetUserId(ClaimsPrincipal user)
        {
            var idStr = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(idStr, out var userId) ? userId : null;
        }

        public static string? GetUserRole(ClaimsPrincipal user) =>
            user.FindFirst(ClaimTypes.Role)?.Value;
    }
}
