using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace AuthService.API.Middlewares
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class HasPermissionAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _permissionKey;

        public HasPermissionAttribute(string permissionKey)
        {
            _permissionKey = permissionKey;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (user?.Identity == null || !user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // ✅ Lấy tất cả các claim permission từ token
            var permissions = user.FindAll("permission").Select(p => p.Value).ToList();

            // ✅ Kiểm tra xem permissionKey có nằm trong danh sách hay không
            if (!permissions.Contains(_permissionKey))
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}
