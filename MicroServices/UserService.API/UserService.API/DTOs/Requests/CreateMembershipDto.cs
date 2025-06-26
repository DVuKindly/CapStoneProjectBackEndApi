namespace UserService.API.DTOs.Requests
{
    public class CreateMembershipDto
    {
        public Guid AccountId { get; set; }

        // ✅ Thêm PackageId (Guid?)
        public Guid PackageId { get; set; }

        public string PackageName { get; set; } = string.Empty;
        public string PackageType { get; set; } = "basic";

        public decimal Amount { get; set; }

        public string? PaymentMethod { get; set; }
        public string? PaymentTransactionId { get; set; }
        public string? PaymentNote { get; set; }

        public bool UsedForRoleUpgrade { get; set; } = false;
        public string? PlanSource { get; set; }

        public int PackageDurationValue { get; set; }   
        public string PackageDurationUnit { get; set; } = string.Empty; 


        public DateTime? ExpireAt { get; set; }
    }
}
