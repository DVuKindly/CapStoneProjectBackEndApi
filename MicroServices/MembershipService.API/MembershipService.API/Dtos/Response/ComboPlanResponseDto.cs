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
        public Guid? PropertyId { get; set; }
        public string PropertyName { get; set; }
        public int BasicPlanCategoryId { get; set; }
        public string BasicPlanCategoryName { get; set; }

        public int PlanLevelId { get; set; }
        public string PlanLevelName { get; set; }

        public int TargetAudienceId { get; set; }
        public string TargetAudienceName { get; set; }


        public List<Guid> BasicPlanIds { get; set; }
        public List<PackageDurationDto> PackageDurations { get; set; }
    }
}