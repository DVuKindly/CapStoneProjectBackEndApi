using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RedisProxy
{
    public interface IRedisSidecarProxy
    {
        Task SetUserProfileCache(UserProfileDto user);
        Task RemoveUserProfileCache(string userId);
    }
    public class UserProfileDto
    {
        public string AccountId { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string RoleType { get; set; } = null!;
        public string Location { get; set; } = null!;
    }
}
