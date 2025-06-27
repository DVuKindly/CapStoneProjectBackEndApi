namespace UserService.API.DTOs.Requests
{
    public class MarkPaidRequestDto
    {
        public Guid RequestId { get; set; } // Chính là Id của Membership hoặc PendingMembershipRequest
        public Guid? MembershipRequestId { get; set; } // Reserved - nếu cần mapping thêm
        public bool? IsDirectMembership { get; set; } // true: mua trực tiếp (BasicPlan), false: combo (cần nâng role)
        public string? PaymentMethod { get; set; }
        public string? PaymentTransactionId { get; set; }
        public string? PaymentNote { get; set; }
        public string? PaymentProofUrl { get; set; }
        public DateTime? PaidTime { get; set; }
    }



}
