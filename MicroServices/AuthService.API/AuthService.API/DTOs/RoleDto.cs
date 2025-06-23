namespace AuthService.API.DTOs
{
    public class RoleDto
    {
        public Guid RoleId { get; set; }
        public string RoleKey { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
