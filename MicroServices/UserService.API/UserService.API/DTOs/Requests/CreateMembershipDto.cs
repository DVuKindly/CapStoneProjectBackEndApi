namespace UserService.API.DTOs.Requests
{
    public class CreateMembershipDto
    {
        public Guid AccountId { get; set; }
        public string PackageName { get; set; } = string.Empty;
        public string PackageType { get; set; } = "basic";
        public decimal Amount { get; set; }
        public string? PaymentMethod { get; set; }
        public bool UsedForRoleUpgrade { get; set; } = false;
        public string? PlanSource { get; set; }

        // ✅ Thêm duration:
        public int PackageDurationValue { get; set; }
        public string PackageDurationUnit { get; set; } = "month"; // day | month | year
    }

}
