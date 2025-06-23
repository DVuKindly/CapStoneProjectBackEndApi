using AuthService.API.DTOs;
using AuthService.API.DTOs.AdminCreate;
using AuthService.API.DTOs.Request;
using AuthService.API.DTOs.Responses;
using AuthService.API.Middlewares;
using AuthService.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers
{
    [Route("api/auth/super-admin")]
    [ApiController]
    [Authorize(Roles = "super_admin")]
    public class SuperAdminAuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public SuperAdminAuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register-admin")]
        [HasPermission("create_admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterBySuperAdminRequest request)
        {
            var result = await _authService.RegisterAdminAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("assign-role")]
        [HasPermission("assign_role")]
        public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleRequest request)
        {
            var result = await _authService.AssignRoleAsync(request.UserId, request.RoleId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("change-user-role")]
        [HasPermission("change_role")]
        public async Task<IActionResult> ChangeUserRole([FromBody] ChangeUserRoleRequest request)
        {
            var result = await _authService.ChangeUserRoleAsync(request.UserId, request.NewRoleId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("assign-permission-to-role")]
        [HasPermission("assign_permission_to_role")]
        public async Task<IActionResult> AssignPermissionToRole([FromBody] AssignPermissionToRoleRequest request)
        {
            var result = await _authService.AssignPermissionToRoleAsync(request.RoleId, request.PermissionId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("assign-permission-to-user")]
        [HasPermission("assign_permission_to_user")]
        public async Task<IActionResult> AssignPermissionToUser([FromBody] AssignPermissionToUserRequest request)
        {
            var result = await _authService.AssignPermissionToUserAsync(request.UserId, request.PermissionId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("user/{userId}/roles")]
        public async Task<IActionResult> GetUserRoles(Guid userId)
        {
            var roles = await _authService.GetUserRolesAsync(userId);
            return Ok(roles);
        }

        [HttpGet("get-all-roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _authService.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpGet("get-all-permissions")]
        public async Task<IActionResult> GetAllPermissions()
        {
            var permissions = await _authService.GetAllPermissionsAsync();
            return Ok(permissions);
        }

        [HttpGet("available-locations")]
        public async Task<IActionResult> GetAvailableLocations()
        {
            var locations = await _authService.GetLocationsAsync();
            return Ok(locations);
        }
    }

    public class ChangeUserRoleRequest
    {
        public Guid UserId { get; set; }
        public Guid NewRoleId { get; set; }
    }
}
