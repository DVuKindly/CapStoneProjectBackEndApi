using AuthService.API.DTOs.AdminCreate;
using AuthService.API.DTOs;
using AuthService.API.Middlewares;
using AuthService.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers
{
    [Route("api/auth/admin")]
    [ApiController]
    public class AdminAuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AdminAuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // ✅ Admin tạo Manager
        [HttpPost("register/manager")]
        [HasPermission("create_manager")]
        public async Task<IActionResult> RegisterManager([FromBody] RegisterStaffRequest request)
        {
            request.RoleKey = "manager";
            var result = await _authService.RegisterManagerAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // ✅ Admin tạo Staff Onboarding
        [HttpPost("register/staff-onboarding")]
        [HasPermission("create_staff_onboarding")]
        public async Task<IActionResult> RegisterStaffOnboarding([FromBody] RegisterStaffRequest request)
        {
            request.RoleKey = "staff_onboarding";
            var result = await _authService.RegisterStaffAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // ✅ Admin tạo Staff Service
        [HttpPost("register/staff-service")]
        [HasPermission("create_staff_service")]
        public async Task<IActionResult> RegisterStaffService([FromBody] RegisterStaffRequest request)
        {
            request.RoleKey = "staff_service";
            var result = await _authService.RegisterStaffAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // ✅ Admin tạo Staff Content
        [HttpPost("register/staff-content")]
        [HasPermission("create_staff_content")]
        public async Task<IActionResult> RegisterStaffContent([FromBody] RegisterStaffRequest request)
        {
            request.RoleKey = "staff_content";
            var result = await _authService.RegisterStaffAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // ✅ Admin tạo Coach
        [HttpPost("register/coach")]
        [HasPermission("create_coach")]
        public async Task<IActionResult> RegisterCoach([FromBody] RegisterCoachRequest request)
        {
            var result = await _authService.RegisterCoachAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // ✅ Admin tạo Partner
        [HttpPost("register/partner")]
        [HasPermission("create_partner")]
        public async Task<IActionResult> RegisterPartner([FromBody] RegisterPartnerRequest request)
        {
            var result = await _authService.RegisterPartnerAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // ✅ Admin tạo Supplier
        [HttpPost("register/supplier")]
        [HasPermission("create_supplier")]
        public async Task<IActionResult> RegisterSupplier([FromBody] RegisterSupplierRequest request)
        {
            var result = await _authService.RegisterSupplierAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // ✅ Gán Role cho User
        [HttpPost("assign-role")]
        [HasPermission("assign_role")]
        public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleRequest request)
        {
            var result = await _authService.AssignRoleAsync(request.UserId, request.RoleId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // ✅ Gán Permission cho Role
        [HttpPost("assign-permission-to-role")]
        [HasPermission("assign_permission_to_role")]
        public async Task<IActionResult> AssignPermissionToRole([FromBody] AssignPermissionToRoleRequest request)
        {
            var result = await _authService.AssignPermissionToRoleAsync(request.RoleId, request.PermissionId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // ✅ Lấy danh sách Role của User
        [HttpGet("user/{userId}/roles")]
        [HasPermission("assign_role")]
        public async Task<IActionResult> GetUserRoles(Guid userId)
        {
            var roles = await _authService.GetUserRolesAsync(userId);
            return Ok(roles);
        }

        // ✅ Lấy tất cả Roles
        [HttpGet("get-all-roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _authService.GetAllRolesAsync();
            return Ok(roles);
        }

        // ✅ Lấy tất cả Permissions
        [HttpGet("get-all-permissions")]
        public async Task<IActionResult> GetAllPermissions()
        {
            var permissions = await _authService.GetAllPermissionsAsync();
            return Ok(permissions);
        }

    }
}
