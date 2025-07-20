using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UserService.API.Entities;

[Table("PartnerProfiles")]
public class PartnerProfile
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid AccountId { get; set; }

    [MaxLength(255)]
    public string? OrganizationName { get; set; }

    [MaxLength(100)]
    public string? PartnerType { get; set; }

    public Guid? LocationId { get; set; }

    [MaxLength(255)]
    public string? ContractUrl { get; set; }

    public bool IsActivated { get; set; } = false;

    public DateTime? ActivatedAt { get; set; }

    public Guid? CreatedByAdminId { get; set; }

    [MaxLength(100)]
    public string? RepresentativeName { get; set; }

    [MaxLength(100)]
    public string? RepresentativePhone { get; set; }

    [MaxLength(100)]
    public string? RepresentativeEmail { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    [MaxLength(255)]
    public string? WebsiteUrl { get; set; }

    [MaxLength(100)]
    public string? Industry { get; set; }

    // 🔁 Navigation
    [ForeignKey("AccountId")]
    public virtual UserProfile? UserProfile { get; set; }

    [ForeignKey("LocationId")]
    public virtual Property? Property { get; set; }
}
