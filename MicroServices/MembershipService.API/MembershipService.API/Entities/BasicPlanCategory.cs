namespace MembershipService.API.Entities
{
    public class PlanCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public ICollection<BasicPlan> BasicPlans { get; set; }
        public ICollection<ComboPlan> ComboPlans { get; set; }
    }
}