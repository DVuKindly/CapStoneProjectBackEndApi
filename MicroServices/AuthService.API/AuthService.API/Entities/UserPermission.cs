using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.API.Entities
{
    public class UserPermission
    {
        public Guid UserId { get; set; }
        public UserAuth User { get; set; }

        public Guid PermissionId { get; set; }
        public Permission Permission { get; set; }
    }
}
