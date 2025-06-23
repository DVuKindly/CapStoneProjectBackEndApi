namespace UserService.API.DTOs.Requests
{
    public class MembershipDto
    {
        public Guid MembershipId { get; set; }
        public string PackageName { get; set; }
        public string PackageType { get; set; }
        public DateTime PurchasedAt { get; set; }
        public int DurationInMonths { get; set; } // phải lưu duration nếu chưa có
        public DateTime ExpiredAt => PurchasedAt.AddMonths(DurationInMonths);
        public bool IsExpired => DateTime.UtcNow > ExpiredAt;
        public int DaysLeft => (ExpiredAt - DateTime.UtcNow).Days;
        public bool UsedForRoleUpgrade { get; set; }
    }

}
