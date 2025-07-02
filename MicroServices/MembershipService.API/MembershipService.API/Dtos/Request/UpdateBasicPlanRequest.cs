namespace MembershipService.API.Dtos.Request
{
    public class UpdateBasicPlanRequest
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool VerifyBuy { get; set; }

        public int BasicPlanCategoryId { get; set; }
        public int PlanLevelId { get; set; }
        public int TargetAudienceId { get; set; }

    }
}
