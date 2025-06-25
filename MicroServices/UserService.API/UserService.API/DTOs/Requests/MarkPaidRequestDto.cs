namespace UserService.API.DTOs.Requests
{
    public class MarkPaidRequestDto
    {
        public Guid RequestId { get; set; } // Id của PendingMembershipRequests (combo)
        public Guid? MembershipRequestId { get; set; } // Dùng cho basic (có thể null combo)
        public bool? IsDirectMembership { get; set; } // false = combo
        public string? PaymentMethod { get; set; }
        public string? PaymentTransactionId { get; set; }
        public string? PaymentNote { get; set; }
        public string? PaymentProofUrl { get; set; }
    }


}
