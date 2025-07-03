namespace MembershipService.API.Entities
{
    public class BasicPlanLevel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public Guid BasicPlanTypeId { get; set; }
        public BasicPlanType BasicPlanType { get; set; }

        public ICollection<BasicPlan> BasicPlans { get; set; }

    }
}
