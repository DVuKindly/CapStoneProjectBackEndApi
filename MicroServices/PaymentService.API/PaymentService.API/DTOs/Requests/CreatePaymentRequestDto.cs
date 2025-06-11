namespace PaymentService.API.DTOs.Requests
{
    public class CreatePaymentRequestDto
    {
        public Guid MembershipRequestId { get; set; }
        public string PaymentMethod { get; set; } = "vnpay"; // or momo
        public string? ReturnUrl { get; set; }
    }

}
