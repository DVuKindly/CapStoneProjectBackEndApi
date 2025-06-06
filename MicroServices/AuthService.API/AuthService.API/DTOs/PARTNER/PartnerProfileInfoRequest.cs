using System.ComponentModel.DataAnnotations;

namespace AuthService.API.DTOs.PARTNER
{
    public class PartnerProfileInfoRequest
    {
        [Required, MaxLength(255)]
        public string? OrganizationName { get; set; }

        [MaxLength(100)]
        public string? PartnerType { get; set; }

        [MaxLength(100)]
        public string? Location { get; set; }

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
    }
}
