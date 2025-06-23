namespace AuthService.API.DTOs
{
    public class PermissionDto
    {
        public Guid PermissionId { get; set; }
        public string PermissionKey { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
