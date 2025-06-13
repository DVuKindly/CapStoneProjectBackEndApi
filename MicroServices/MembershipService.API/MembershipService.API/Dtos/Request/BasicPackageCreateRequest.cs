namespace MembershipService.API.Dtos.Request
{
    public class BasicPackageCreateRequest
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public bool VerifyBuy { get; set; }
        public Guid? PackageLevelId { get; set; }
        public int PackageDurationId { get; set; }
        public List<Guid> NextUServiceIds { get; set; }
    }

    public class BasicPackageUpdateRequest : BasicPackageCreateRequest { }
}
