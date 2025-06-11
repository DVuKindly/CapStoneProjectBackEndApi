namespace PaymentService.API.DTOs.Requests
{
    public class WebhookNotificationDto
    {
        public string OrderId { get; set; } = null!;
        public string RequestId { get; set; } = null!;
        public long Amount { get; set; }
        public string ResultCode { get; set; } = null!; // "0" là thanh toán thành công
        public string Message { get; set; } = null!;
        public string Signature { get; set; } = null!;
        public string ExtraData { get; set; } = null!;
    }

}
