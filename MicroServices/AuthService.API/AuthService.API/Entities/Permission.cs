using System.ComponentModel.DataAnnotations;

namespace AuthService.API.Entities
{
    public class Permission
    {
        [Key]
        public Guid PermissionId { get; set; }

        [Required, MaxLength(100)]
        public string PermissionName { get; set; }

        public string? Description { get; set; }

        public ICollection<RolePermission> RolePermissions { get; set; }
        public ICollection<UserPermission> UserPermissions { get; set; }
    }
}
