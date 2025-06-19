using MembershipService.API.Entities.Common;

namespace MembershipService.API.Entities
{
    public class ComboPlan : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal TotalPrice { get; set; }
        public float DiscountRate { get; set; }
        public bool IsSuggested { get; set; }

        public Guid? LocationId { get; set; }
        public Location? Location { get; set; } = null!;

        public Guid PackageLevelId { get; set; }
        public PackageLevel PackageLevel { get; set; }

        public Guid BasicPlanId { get; set; }
        public BasicPlan BasicPlan { get; set; }

        public ICollection<ComboPlanService> ComboPlanServices { get; set; }
    }
}
