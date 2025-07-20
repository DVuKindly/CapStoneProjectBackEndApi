using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.API.Entities
{
    [Table("SupplierProfiles")]
    public class SupplierProfile
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid AccountId { get; set; }

        [MaxLength(100)]
        public string? CompanyName { get; set; }

        [MaxLength(100)]
        public string? ContactPerson { get; set; }

        [MaxLength(50)]
        public string? ContactPhone { get; set; }

        [MaxLength(100)]
        public string? ContactEmail { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [MaxLength(255)]
        public string? WebsiteUrl { get; set; }

        [MaxLength(100)]
        public string? Industry { get; set; }

        [MaxLength(50)]
        public string? TaxCode { get; set; }

        public Guid? LocationId { get; set; }

        // 🔁 Navigation tới UserProfile
        [ForeignKey("AccountId")]
        public virtual UserProfile? UserProfile { get; set; }

        // 🔁 Navigation tới Property
        [ForeignKey("LocationId")]
        public virtual Property? Property { get; set; }
    }
}
