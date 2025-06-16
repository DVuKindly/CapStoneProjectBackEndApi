namespace UserService.API.DTOs.Requests
{
    public class MarkPaidRequestDto
    {
        public Guid RequestId { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentTransactionId { get; set; }
        public string? PaymentNote { get; set; }
        public string? PaymentProofUrl { get; set; }
    }

}
