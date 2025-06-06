using System.ComponentModel.DataAnnotations;

namespace UserService.API.DTOs.Requests
{
    public class CreatePartnerProfileRequest
    {
        [Required]
        public Guid AccountId { get; set; }

        [Required, MaxLength(255)]
        public string? OrganizationName { get; set; }

        [MaxLength(100)]
        public string? PartnerType { get; set; }

        [MaxLength(100)]
        public string? Location { get; set; }

        [MaxLength(500)]
        public string? ContractUrl { get; set; }

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

        // Đã đổi sang GUID
        public Guid? CreatedByAdminId { get; set; }
    }
}
