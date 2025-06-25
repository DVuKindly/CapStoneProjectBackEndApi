namespace PaymentService.API.DTOs.Requests
{
    public class MembershipRequestSummaryDto
    {
        public Guid MembershipRequestId { get; set; }
        public Guid AccountId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } = "PendingPayment";
        public bool IsCombo { get; set; } // ✅ phía UserService gán true nếu lấy từ bảng ComboPlans

    }
}
