namespace PaymentService.API.DTOs.Requests
{
    public class CreatePaymentRequestDto
    {
       
        public Guid RequestId { get; set; }

      
        public string PaymentMethod { get; set; } = "VNPAY";

     
        public string? ReturnUrl { get; set; }
        public bool IsDirectMembership { get; set; } = false;
    }
}
