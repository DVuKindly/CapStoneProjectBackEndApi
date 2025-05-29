namespace AuthService.API.Entities
{
    public class Role
    {
        public Guid RoleId { get; set; }

   
        public string RoleKey { get; set; } = null!;  

        public string RoleName { get; set; } = null!;
        public string? Description { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
