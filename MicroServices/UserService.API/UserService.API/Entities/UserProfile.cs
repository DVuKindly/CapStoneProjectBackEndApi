using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.API.Entities
{
    [Table("UserProfiles")]
    public class UserProfile
    {
        [Key]
        public Guid Id { get; set; } 

        [Required]
        public Guid AccountId { get; set; }

        [MaxLength(100)]
        public string? FullName { get; set; }

        [MaxLength(50)]
        public string? Phone { get; set; }

        [MaxLength(10)]
        public string? Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DOB { get; set; }

        [MaxLength(255)]
        public string? AvatarUrl { get; set; }
        [MaxLength(1000)]
        public string? Interests { get; set; } 

        [MaxLength(1000)]
        public string? PersonalityTraits { get; set; } 

        [MaxLength(2000)]
        public string? Introduction { get; set; } 

        [MaxLength(500)]
        public string? CvUrl { get; set; }
        [MaxLength(255)]
        public string? Address { get; set; }

        [MaxLength(500)]
        public string? SocialLinks { get; set; }

        public Guid? LocationId { get; set; }

        [MaxLength(50)]
        public string? RoleType { get; set; }

        public bool IsCompleted { get; set; } = false;

        public Guid? VerifiedByAdmin { get; set; }

        [MaxLength(50)]
        public string? OnboardingStatus { get; set; }

        [MaxLength(1000)]
        public string? Note { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

      

        public Guid? CreatedByAdminId { get; set; }

        [ForeignKey("LocationId")]
        public virtual LocationRegion? LocationRegion { get; set; }
    }
}
