namespace PaymentService.API.Entities
{
    public class PaymentTransaction
    {
        public Guid Id { get; set; }

        public Guid PaymentRequestId { get; set; }

        public string TransactionId { get; set; } = null!;
        public string Gateway { get; set; } = null!;           // momo | vnpay
        public string GatewayResponse { get; set; } = null!;   // raw JSON string
        public string? Status { get; set; }                    // Success / Failed / Pending

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ConfirmedAt { get; set; }

        public PaymentRequest PaymentRequest { get; set; } = null!;
    }
}
