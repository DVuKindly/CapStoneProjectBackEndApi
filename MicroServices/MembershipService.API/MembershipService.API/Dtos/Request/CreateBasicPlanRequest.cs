namespace MembershipService.API.Dtos.Request
{
    public class CreateBasicPlanRequest
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public bool VerifyBuy { get; set; }
        public Guid PlanCategoryId { get; set; }
        public Guid? LocationId { get; set; }
        public List<Guid> ServiceIds { get; set; } = new();
        public List<PackageDurationDto> PackageDurations { get; set; } = new();
    }

    public class UpdateBasicPlanRequest : CreateBasicPlanRequest { }

    public class PackageDurationDto
    {
        public int DurationId { get; set; }
        public decimal DiscountRate { get; set; }
    }
}
