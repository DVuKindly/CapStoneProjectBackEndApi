using MembershipService.API.Entities.Common;

namespace MembershipService.API.Entities
{
    public class ComboPlanService : AuditableEntity
    {
        public Guid Id { get; set; }

        public Guid ComboPlanId { get; set; }
        public ComboPlan ComboPlan { get; set; }

        public Guid NextUServiceId { get; set; }
        public NextUService NextUService { get; set; }
    }
}
