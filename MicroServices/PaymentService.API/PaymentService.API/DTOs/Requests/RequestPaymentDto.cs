namespace PaymentService.API.DTOs.Requests
{
    public class RequestPaymentDto
    {
        public Guid MembershipRequestId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = "momo"; // hoặc vnpay
        public string? ReturnUrl { get; set; }
    }

}
