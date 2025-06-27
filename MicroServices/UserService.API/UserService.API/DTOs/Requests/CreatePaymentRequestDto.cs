namespace UserService.API.DTOs.Requests
{
    public class CreatePaymentRequestDto
    {
        public Guid RequestId { get; set; }            // ID của Membership hoặc PendingMembershipRequest
        public Guid AccountId { get; set; }            // ID người dùng
        public Guid? PackageId { get; set; }            // ID gói dịch vụ
        public decimal? Amount { get; set; }            // Giá tiền

        public string RedirectUrl { get; set; } = string.Empty;     // URL để redirect sau thanh toán
        public string PaymentMethod { get; set; } = string.Empty;   // VNPAY, MOMO...
        public string PackageType { get; set; } = string.Empty;     // basic hoặc combo
        public bool IsDirectMembership { get; set; } = false;

    }
}
