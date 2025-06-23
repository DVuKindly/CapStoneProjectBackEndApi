namespace AuthService.API.DTOs.Request
{
    public class AssignPermissionToUserRequest
    {
        public Guid UserId { get; set; }
        public Guid PermissionId { get; set; }
    }
}
