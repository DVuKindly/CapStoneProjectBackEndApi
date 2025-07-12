using System.Security.Claims;

namespace AuthService.API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetAccountId(this ClaimsPrincipal user)
        {
            var idClaim = user.FindFirst(ClaimTypes.NameIdentifier); // hoặc "sub", "userId", tuỳ JWT của bạn

            if (idClaim == null || !Guid.TryParse(idClaim.Value, out var accountId))
                throw new UnauthorizedAccessException("Invalid or missing AccountId claim.");

            return accountId;
        }
    }
}
