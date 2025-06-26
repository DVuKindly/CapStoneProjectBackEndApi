using MembershipService.API.Dtos.Request;

namespace MembershipService.API.Dtos.Response
{
    public class ComboPlanResponseDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal DiscountRate { get; set; }
        public bool IsSuggested { get; set; }
        public bool VerifyBuy { get; set; }
        public Guid? LocationId { get; set; }
        public string LocationName { get; set; }
        public Guid PackageLevelId { get; set; }
        public string PackageLevelName { get; set; }

        public List<Guid> BasicPlanIds { get; set; }
        public List<PackageDurationDto> PackageDurations { get; set; }
    }
}
