namespace PaymentService.API.DTOs.Requests
{
    public class MarkPaidRequestDto
    {
        public Guid RequestId { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string PaymentTransactionId { get; set; } = string.Empty;
        public string? PaymentNote { get; set; }
        public string? PaymentProofUrl { get; set; }
    }
}
