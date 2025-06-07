using System.ComponentModel.DataAnnotations;

namespace AuthService.API.Entities
{
    public class UserAuth
    {
        [Key]
        public Guid UserId { get; set; }

        [Required, MaxLength(100)]
        public string UserName { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        public string? PasswordHash { get; set; }

        public string? Location { get; set; }

        public bool EmailVerified { get; set; } = false;
        public string? EmailVerificationToken { get; set; }
        public DateTime? EmailVerificationExpiry { get; set; }

        public string? ResetPasswordToken { get; set; }
        public DateTime? ResetPasswordTokenExpiry { get; set; }

        // ✅ Thêm token dành cho external user tạo tài khoản nhưng chưa đặt pass
        public string? SetPasswordToken { get; set; }
        public DateTime? SetPasswordTokenExpiry { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }

        public int LoginAttempt { get; set; } = 0;
        public bool IsLocked { get; set; } = false;

        public string? Provider { get; set; }
        public string? ProviderId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
    }

}
