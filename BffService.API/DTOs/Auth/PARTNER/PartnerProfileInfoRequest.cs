using BffService.API.DTOs.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BffService.API.DTOs.PARTNER
{
    public class PartnerProfileInfoRequest : IProfileInfoRequest
    {
        [Required, MaxLength(255)]
        public string OrganizationName { get; set; } = null!;

        [MaxLength(100)]
        public string? PartnerType { get; set; }

        [Required]
        public Guid LocationId { get; set; }

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
        public string Note {  get; set; }
    }
}
