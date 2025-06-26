using MembershipService.API.Dtos.Request;

namespace MembershipService.API.Dtos.Response
{
    public class BasicPlanResponseDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public bool VerifyBuy { get; set; }
        public Guid PlanCategoryId { get; set; }
        public string PlanCategoryName { get; set; }
        public Guid? LocationId { get; set; }
        public string? LocationName { get; set; }
        public List<Guid> ServiceIds { get; set; }
        public List<PackageDurationDto> PackageDurations { get; set; }
    }
}
