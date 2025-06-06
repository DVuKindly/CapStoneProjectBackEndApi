using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using UserService.API.Entities;

public class PendingMembershipRequest
{
    public Guid Id { get; set; }

    [Required]
    public Guid AccountId { get; set; }

    [ForeignKey(nameof(AccountId))]
    public UserProfile? UserProfile { get; set; }

    [Required]
    public Guid PackageId { get; set; } 

  

    [Required, MaxLength(255)]
    public string RequestedPackageName { get; set; } = null!;

    [Required, MaxLength(100)]
    public string Location { get; set; } = null!;

    [Required, MaxLength(50)]
    public string Status { get; set; } = null!;

    [MaxLength(1000)]
    public string? StaffNote { get; set; }

    public Guid? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
