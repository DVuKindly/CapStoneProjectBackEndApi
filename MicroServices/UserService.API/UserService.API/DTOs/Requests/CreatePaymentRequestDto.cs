namespace UserService.API.DTOs.Requests
{
    public class CreatePaymentRequestDto
    {
        public Guid RequestId { get; set; }        // ID từ PendingMembershipRequest
        public Guid AccountId { get; set; }        // ID người dùng
        public Guid PackageId { get; set; }        // Gói đăng ký
        public decimal Amount { get; set; }        // Giá tiền (lấy từ MembershipPackageService)
        public string RedirectUrl { get; set; }    // URL frontend redirect sau khi thanh toán
    }

}
