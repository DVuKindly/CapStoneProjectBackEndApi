namespace UserService.API.DTOs.Requests
{
    public class PaymentWebhookDto
    {
        public Guid RequestId { get; set; }
        public Guid AccountId { get; set; }
        public string TransactionId { get; set; }
        public string PaymentGateway { get; set; }
    }

}
