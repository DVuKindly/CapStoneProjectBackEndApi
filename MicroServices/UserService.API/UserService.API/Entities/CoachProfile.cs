using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.API.Entities
{
    public class CoachProfile
    {
        public Guid Id { get; set; }

        [Required, MaxLength(100)]
        public string AccountId { get; set; } = null!;

        [ForeignKey(nameof(AccountId))]
        public UserProfile? UserProfile { get; set; }

        [MaxLength(100)]
        public string? CoachType { get; set; }

        [MaxLength(255)]
        public string? Specialty { get; set; }

        [MaxLength(255)]
        public string? ModuleInCharge { get; set; }

        [MaxLength(100)]
        public string? Region { get; set; }
    }
}
