namespace MembershipService.API.Dtos.Response
{
    public class BasicPackageResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public bool VerifyBuy { get; set; }

        public Guid? PackageLevelId { get; set; }
        public string? PackageLevelName { get; set; }

        public int PackageDurationId { get; set; }
        public string? PackageDurationName { get; set; }

        public List<Guid> NextUServiceIds { get; set; }
    }
}
