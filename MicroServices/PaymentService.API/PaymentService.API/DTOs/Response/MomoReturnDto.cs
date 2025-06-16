namespace PaymentService.API.DTOs.Response
{
    public class MomoReturnDto
    {
        public Guid PaymentRequestId { get; set; }
        public string PaymentStatus { get; set; } = string.Empty; 
        public string? Message { get; set; }
        public string? ResultCode { get; set; }
    }
}
