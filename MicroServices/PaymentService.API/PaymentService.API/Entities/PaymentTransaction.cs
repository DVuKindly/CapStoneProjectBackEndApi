using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentService.API.Entities
{
    public class PaymentTransaction
    {
        public Guid Id { get; set; }

        [Required]
        public Guid PaymentRequestId { get; set; }

        [Required]
        public string TransactionId { get; set; } = null!; // Mã giao dịch VNPay (vnp_TransactionNo)

        [Required]
        public string Gateway { get; set; } = "vnpay"; // momo | vnpay

        [Required]
        public string GatewayResponse { get; set; } = ""; // Raw JSON hoặc QueryString để trace

        public string? Status { get; set; } // Success | Failed | Pending

        public string? BankCode { get; set; } // vnp_BankCode
        public DateTime? PayDate { get; set; } // Ngày thanh toán từ vnp_PayDate

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ConfirmedAt { get; set; }

        // 🔁 Navigation
        public PaymentRequest PaymentRequest { get; set; } = null!;
    }
}
