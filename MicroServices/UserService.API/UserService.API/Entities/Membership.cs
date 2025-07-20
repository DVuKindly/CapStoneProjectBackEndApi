using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace UserService.API.Entities
{
    public class Membership : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid AccountId { get; set; }

        [Required]
        public Guid PackageId { get; set; }

        [Required]
        public string PackageType { get; set; } = "basic";

        [Required]
        public string PackageName { get; set; } = string.Empty;

        [Required]
        public decimal Amount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? AddOnsFee { get; set; }

        public DateTime PurchasedAt { get; set; } = DateTime.UtcNow;
        public DateTime? StartDate { get; set; } = DateTime.UtcNow;
        public DateTime? ExpireAt { get; set; }

        public string? PaymentStatus { get; set; }
        public string? PaymentMethod { get; set; }
        public int? PackageDurationValue { get; set; }
        public string? PackageDurationUnit { get; set; }

        public string? PaymentTransactionId { get; set; }
        public DateTime? PaymentTime { get; set; }
        public string? PaymentNote { get; set; }

        public Guid LocationId { get; set; }
        public bool UsedForRoleUpgrade { get; set; } = false;

        public string? PlanSource { get; set; }
        public Guid? RoomInstanceId { get; set; }
        public Guid? BookingId { get; set; }

        public Guid? UserProfileId { get; set; }
        public UserProfile? UserProfile { get; set; }

        // ✅ NEW: Liên kết với yêu cầu
        public Guid? PendingRequestId { get; set; }
        public PendingMembershipRequest? PendingRequest { get; set; }
      
        [NotMapped]
        public bool IsActive => ExpireAt > DateTime.UtcNow;
    }

}
