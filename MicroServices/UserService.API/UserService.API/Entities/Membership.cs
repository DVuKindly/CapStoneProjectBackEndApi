using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using UserService.API.Entities;

public class Membership
{
    [Key]
    public Guid Id { get; set; }

    public Guid AccountId { get; set; }

    public Guid PackageId { get; set; }

    public string PackageType { get; set; } = "basic";

    public string PackageName { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public DateTime PurchasedAt { get; set; } = DateTime.UtcNow;

    public string? PaymentStatus { get; set; }

    public string? PaymentMethod { get; set; }
    public int? PackageDurationValue { get; set; }
    public string? PackageDurationUnit { get; set; }  // "day", "month", "year"

    public string? PaymentTransactionId { get; set; }

    public DateTime? PaymentTime { get; set; }

    public string? PaymentNote { get; set; }

    public Guid LocationId { get; set; }

    public bool UsedForRoleUpgrade { get; set; } = false;

    public DateTime? ExpireAt { get; set; }

    [NotMapped]
    public bool IsActive => !ExpireAt.HasValue || ExpireAt > DateTime.UtcNow;

    public string? PlanSource { get; set; }

    public Guid? UserProfileId { get; set; }
    public UserProfile? UserProfile { get; set; }
}
