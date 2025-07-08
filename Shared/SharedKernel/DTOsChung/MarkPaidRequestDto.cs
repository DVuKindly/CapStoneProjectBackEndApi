namespace SharedKernel.DTOsChung
{
    public enum PaymentSource
    {
        PendingMembershipRequest,
        Membership
    }

    public class MarkPaidRequestDto
    {
        // id payment
        public Guid? RequestId { get; set; }
        // id packgake
        public Guid? MembershipRequestId { get; set; }

        public string PaymentTransactionId { get; set; } = string.Empty;

        public string PaymentMethod { get; set; } = string.Empty;

        public string? PaymentNote { get; set; }

        public string? PaymentProofUrl { get; set; }

        public string? FullName { get; set; }
        public DateTime? StartDate { get; set; }
        public PaymentSource Source { get; set; } = PaymentSource.PendingMembershipRequest;

    
    }

}
