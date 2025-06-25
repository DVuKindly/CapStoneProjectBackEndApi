using System.ComponentModel.DataAnnotations;

namespace PaymentService.API.Entities
{
    public class PaymentRequest
    {
        public Guid Id { get; set; }

        [Required]
        public string RequestCode { get; set; } = Guid.NewGuid().ToString("N")[..18];


        [Required]
        public Guid AccountId { get; set; }

        [Required]
        public Guid MembershipRequestId { get; set; } // Liên kết với UserService

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string PaymentMethod { get; set; } = "momo"; // momo | vnpay
                                                           
        public bool IsDirectMembership { get; set; } = false;

        public string Status { get; set; } = "Pending";
        // Possible values: Pending, Paid, Failed, Expired

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ExpireAt { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // ✅ Real-time support
        public bool IsWebhookHandled { get; set; } = false; // Đã nhận & xử lý webhook
        public DateTime? WebhookHandledAt { get; set; }

        public bool IsUserServiceUpdated { get; set; } = false;
        public bool IsAuthServiceUpdated { get; set; } = false;

        public string? ReturnUrl { get; set; } // redirect FE nếu cần
        public string? ExtraData { get; set; } // Truyền callback token nếu cần

        public string? FailureReason { get; set; }  // Nếu thất bại
        public string? FailureCode { get; set; }

        // Optional: Redis support nếu dùng Sidecar
        public string? RedisKey { get; set; }
        public bool IsSyncedToRedis { get; set; } = false;
        public DateTime? LastSyncedAt { get; set; }

        // 🔁 Navigation
        public ICollection<PaymentTransaction> Transactions { get; set; } = new List<PaymentTransaction>();
    }
}
