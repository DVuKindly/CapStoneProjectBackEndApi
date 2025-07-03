namespace MembershipService.API.Entities
{
    public class BasicPlanCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public Guid BasicPlanTypeId { get; set; }
        public BasicPlanType BasicPlanType { get; set; }

        public ICollection<BasicPlan> BasicPlans { get; set; }
    }
}
