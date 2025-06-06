using System.ComponentModel.DataAnnotations;

namespace UserService.API.DTOs.Requests
{
    public class CreateCoachProfileRequest
    {
        [Required]
        public Guid AccountId { get; set; }

        [Required, MaxLength(100)]
        public string? CoachType { get; set; }

        [MaxLength(255)]
        public string? Specialty { get; set; }

        [MaxLength(255)]
        public string? ModuleInCharge { get; set; }

        [MaxLength(100)]
        public string? Region { get; set; }

        [Range(0, 100)]
        public int? ExperienceYears { get; set; }

        [MaxLength(1000)]
        public string? Bio { get; set; }

        [MaxLength(500)]
        public string? Certifications { get; set; }

        [MaxLength(255)]
        public string? LinkedInUrl { get; set; }
    }
}
