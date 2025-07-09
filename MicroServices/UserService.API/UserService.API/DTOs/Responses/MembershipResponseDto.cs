namespace UserService.API.DTOs.Responses
{
    public class MembershipResponseDto
    {
        public Guid Id { get; set; }
        public Guid? PendingRequestId { get; set; }

        public string PackageName { get; set; } = string.Empty;
        public string PackageType { get; set; } = "basic";
        public decimal Amount { get; set; }
        public string PaymentStatus { get; set; } = "Pending";
        public string? PaymentMethod { get; set; }

        public DateTime PurchasedAt { get; set; } // ✔️ vì entity là DateTime
        public DateTime? ExpireAt { get; set; }   // ✔️ vì entity là DateTime?

        public bool IsActive { get; set; }
        public bool UsedForRoleUpgrade { get; set; }
        public string? PlanSource { get; set; }
        public DateTime? StartDate { get; set; }


        public int? PackageDurationValue { get; set; }
        public string? PackageDurationUnit { get; set; }

    }


}
