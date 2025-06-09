using BffService.API.DTOs.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BffService.API.DTOs.SUPPLIER
{
    public class SupplierProfileInfoRequest : IProfileInfoRequest
    {
        [Required, MaxLength(255)]
        public string? CompanyName { get; set; }

        [MaxLength(100)]
        public string? TaxCode { get; set; }

        [Required]
        public Guid LocationId { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [MaxLength(255)]
        public string? WebsiteUrl { get; set; }

        [MaxLength(255)]
        public string? ContactPerson { get; set; }

        [MaxLength(100)]
        public string? ContactPhone { get; set; }

        [MaxLength(100)]
        public string? ContactEmail { get; set; }

        [MaxLength(255)]
        public string? Industry { get; set; }
    }
}
