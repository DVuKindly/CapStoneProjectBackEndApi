using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UserService.API.Entities;

[Table("CoachProfiles")]
public class CoachProfile
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid AccountId { get; set; }

    [MaxLength(100)]
    public string? CoachType { get; set; }

    [MaxLength(255)]
    public string? Specialty { get; set; }

    [MaxLength(255)]
    public string? ModuleInCharge { get; set; }

    [MaxLength(100)]
    public string? Region { get; set; }

    public int? ExperienceYears { get; set; }

    [MaxLength(1000)]
    public string? Bio { get; set; }

    [MaxLength(500)]
    public string? Certifications { get; set; }

    [MaxLength(255)]
    public string? LinkedInUrl { get; set; }

    // 🔁 Navigation
    [ForeignKey("AccountId")]
    public virtual UserProfile? UserProfile { get; set; }
}
