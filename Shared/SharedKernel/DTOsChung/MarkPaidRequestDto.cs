namespace SharedKernel.DTOsChung
{
    public class MarkPaidRequestDto
    {
        public Guid RequestId { get; set; }

        // Nếu có hệ thống cũ cần MembershipRequestId riêng thì có thể để thêm, còn không thì bỏ
        // public Guid? MembershipRequestId { get; set; }

        public string PaymentTransactionId { get; set; } = string.Empty;
        // ✅ Thêm dòng này để fix lỗi hệ thống cũ
        public Guid? MembershipRequestId { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;

        public string? PaymentNote { get; set; }

        public string? PaymentProofUrl { get; set; }

        public string? FullName { get; set; }

        public string Source { get; set; } = "PendingMembershipRequest"; // hoặc "Membership"
        public bool? IsDirectMembership { get; set; }


    }
}
