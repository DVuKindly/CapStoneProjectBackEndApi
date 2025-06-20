using MembershipService.API.Entities.Common;

namespace MembershipService.API.Entities
{
    public class BasicPlanService : AuditableEntity
    {
        public Guid Id { get; set; }

        // FK đến BasicPackage
        public Guid BasicPlanId { get; set; }
        public BasicPlan BasicPlan { get; set; }

        // FK đến dịch vụ
        public Guid NextUServiceId { get; set; }
        public NextUService NextUService { get; set; }
    }
}
