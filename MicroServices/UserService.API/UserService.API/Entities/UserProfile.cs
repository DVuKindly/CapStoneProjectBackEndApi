using System.ComponentModel.DataAnnotations;

namespace UserService.API.Entities
{
    public class UserProfile
    {
        public Guid Id { get; set; }

        [Required, MaxLength(100)]
        public string AccountId { get; set; } = null!;

        [Required, MaxLength(255)]
        public string FullName { get; set; } = null!;

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(10)]
        public string? Gender { get; set; }

        public DateTime? DOB { get; set; }

        [MaxLength(500)]
        public string? AvatarUrl { get; set; }

        [MaxLength(1000)]
        public string? SocialLinks { get; set; }

        [MaxLength(100)]
        public string? Location { get; set; }

        [Required, MaxLength(50)]
        public string RoleType { get; set; } = null!;

        public bool IsCompleted { get; set; }
        public bool IsVerifiedByAdmin { get; set; }

        [MaxLength(50)]
        public string? OnboardingStatus { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


        public ICollection<PartnerProfile>? PartnerProfiles { get; set; }
        public ICollection<CoachProfile>? CoachProfiles { get; set; }
        public ICollection<StaffProfile>? StaffProfiles { get; set; }
        public ICollection<PendingMembershipRequest>? PendingMembershipRequests { get; set; }

    }
}
