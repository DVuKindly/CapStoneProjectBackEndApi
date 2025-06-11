namespace PaymentService.API.DTOs.Requests
{
    public class PaymentResultDto
    {
        public string PaymentUrl { get; set; } = string.Empty;
        public string RequestCode { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
    }

}
