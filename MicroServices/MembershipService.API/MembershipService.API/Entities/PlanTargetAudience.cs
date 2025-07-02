namespace MembershipService.API.Entities
{
    public class PlanTargetAudience
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<BasicPlan> BasicPlans { get; set; }
    }
}
