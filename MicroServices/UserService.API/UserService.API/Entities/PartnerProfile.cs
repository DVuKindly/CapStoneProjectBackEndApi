using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.API.Entities
{
    public class PartnerProfile
    {
        public Guid Id { get; set; }

        [Required]
        public Guid AccountId { get; set; }

        [ForeignKey(nameof(AccountId))]
        public UserProfile? UserProfile { get; set; }

        [MaxLength(255)]
        public string? OrganizationName { get; set; }

        [MaxLength(100)]
        public string? PartnerType { get; set; }

        [MaxLength(100)]
        public string? Location { get; set; }

        [MaxLength(500)]
        public string? ContractUrl { get; set; }

        public bool IsActivated { get; set; }
        public DateTime? ActivatedAt { get; set; }

        public Guid? CreatedByAdminId { get; set; }

        [MaxLength(255)]
        public string? RepresentativeName { get; set; }

        [MaxLength(100)]
        public string? RepresentativePhone { get; set; }

        [MaxLength(100)]
        public string? RepresentativeEmail { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [MaxLength(255)]
        public string? WebsiteUrl { get; set; }

        [MaxLength(255)]
        public string? Industry { get; set; }

        public DateTime? JoinedAt { get; set; }
    }
}
