namespace AuthService.API.DTOs.Responses
{
    public class AccountResponseDto
    {
        public Guid AccountId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool EmailVerified { get; set; }

        public string RoleKey { get; set; }
        public string RoleName { get; set; }
        public string RoleType { get; set; } // ✅ Gợi ý thêm

        public string Description { get; set; }

        public Guid? LocationId { get; set; } // ✅ Gợi ý thêm
        public string LocationName { get; set; }

        public bool IsLocked { get; set; }
        public int LoginAttempt { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }

        public string? AvatarUrl { get; set; } // ✅ Gợi ý thêm
        public DateTime CreatedAt { get; set; } // ✅ Gợi ý thêm
    }



}
