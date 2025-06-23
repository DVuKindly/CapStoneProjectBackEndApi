namespace UserService.API.DTOs.Requests
{
    public class PaymentCallbackDto
    {
        public string PaymentMethod { get; set; } = string.Empty;
        public string PaymentTransactionId { get; set; } = string.Empty;
        public string? PaymentNote { get; set; }
    }

}
