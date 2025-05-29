﻿namespace AuthService.API.Entities
{
    public class UserRole
    {
        public Guid UserId { get; set; }
        public UserAuth User { get; set; }

        public Guid RoleId { get; set; }
        public Role Role { get; set; }
    }
}
