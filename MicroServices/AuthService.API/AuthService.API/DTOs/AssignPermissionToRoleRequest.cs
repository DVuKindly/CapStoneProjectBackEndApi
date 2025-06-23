namespace AuthService.API.DTOs
{
    public class AssignPermissionToRoleRequest
    {
        public Guid RoleId { get; set; }
        public Guid PermissionId { get; set; }
    }

}
