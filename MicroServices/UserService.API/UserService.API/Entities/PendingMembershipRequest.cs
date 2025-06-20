using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UserService.API.Entities;

[Table("PendingMembershipRequests")]
public class PendingMembershipRequest
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid AccountId { get; set; }

    public Guid? PackageId { get; set; }

    [MaxLength(255)]
    public string? RequestedPackageName { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? Amount { get; set; }

    [MaxLength(1000)]
    public string? Interests { get; set; }

    [MaxLength(1000)]
    public string? PersonalityTraits { get; set; }

    [MaxLength(2000)]
    public string? Introduction { get; set; }

    [MaxLength(500)]
    public string? CvUrl { get; set; }

    [MaxLength(2000)]
    public string? MessageToStaff { get; set; }

    public Guid? LocationId { get; set; }

    [MaxLength(50)]
    public string? Status { get; set; }

    [MaxLength(1000)]
    public string? StaffNote { get; set; }

    public DateTime? ApprovedAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Thanh toán
    [MaxLength(50)]
    public string? PaymentMethod { get; set; }

    [MaxLength(50)]
    public string? PaymentStatus { get; set; } = "Pending";

    public DateTime? PaymentTime { get; set; }

    [MaxLength(100)]
    public string? PaymentTransactionId { get; set; }
    public string PackageType { get; set; } = null!;


    [MaxLength(1000)]
    public string? PaymentNote { get; set; }

    [MaxLength(1000)]
    public string? PaymentProofUrl { get; set; }

    // 🔁 Navigation
    [ForeignKey("AccountId")]
    public virtual UserProfile? UserProfile { get; set; }

    
}
