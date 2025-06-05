using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace UserService.API.Entities
{
    public class PartnerProfile
    {
        public Guid Id { get; set; }

        [Required, MaxLength(100)]
        public string AccountId { get; set; } = null!;

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

        [MaxLength(100)]
        public string? CreatedByAdminId { get; set; }
    }
}
