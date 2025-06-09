using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UserService.API.Entities;

[Table("PendingThirdPartyRequests")]
public class PendingThirdPartyRequest
{
    [Key]
    public Guid Id { get; set; } 

    [Required, MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? FullName { get; set; }

    [MaxLength(50)]
    public string? RoleType { get; set; }

    public Guid? LocationId { get; set; }

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    [MaxLength(50)]
    public string Status { get; set; } = "Pending";

    public DateTime? ApprovedAt { get; set; }

    public Guid? ApprovedByManagerId { get; set; }

    [MaxLength(1000)]
    public string? Note { get; set; }

    public string? ProfileDataJson { get; set; }

    // 🔁 Navigation
    [ForeignKey("LocationId")]
    public virtual LocationRegion? LocationRegion { get; set; }
}
