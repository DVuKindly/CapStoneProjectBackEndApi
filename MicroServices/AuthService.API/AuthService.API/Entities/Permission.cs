using System.ComponentModel.DataAnnotations;

namespace AuthService.API.Entities
{
    public class Permission
    {
        [Key]
        public Guid PermissionId { get; set; }

        [Required, MaxLength(100)]
        public string PermissionKey { get; set; }  // ⚠️ đổi tên trường này

        public string? Description { get; set; }

        public ICollection<RolePermission> RolePermissions { get; set; }
        public ICollection<UserPermission> UserPermissions { get; set; }
    }
}
