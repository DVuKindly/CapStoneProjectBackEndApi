using MembershipService.API.Entities.Common;

namespace MembershipService.API.Entities
{
    public class ComboPlanBasic : AuditableEntity
    {
        public Guid Id { get; set; }

        public Guid ComboPlanId { get; set; }
        public ComboPlan ComboPlan { get; set; }

        public Guid BasicPlanId { get; set; }
        public BasicPlan BasicPlan { get; set; }
    }
}
