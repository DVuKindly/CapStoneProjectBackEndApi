namespace MembershipService.API.Entities
{
    public class PlanLevel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<BasicPlan> BasicPlans { get; set; }
        public ICollection<ComboPlan> ComboPlans { get; set; }


    }
}