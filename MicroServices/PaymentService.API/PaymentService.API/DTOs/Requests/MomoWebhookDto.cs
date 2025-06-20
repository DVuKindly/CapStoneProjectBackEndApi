namespace PaymentService.API.DTOs.Requests
{
    public class MomoWebhookDto
    {
        public string RequestCode { get; set; } // chuỗi mã giao dịch từ Momo
        public string TransactionId { get; set; }
        public string PaymentMethod { get; set; } = "MOMO";
        public string PaymentNote { get; set; }
        public string PaymentProofUrl { get; set; }
        public string Status { get; set; }
        public string RawResponse { get; set; } // Nếu cần lưu thô thông tin
    }
}
