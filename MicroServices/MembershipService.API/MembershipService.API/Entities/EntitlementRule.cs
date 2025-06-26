using MembershipService.API.Entities.Common;
using MembershipService.API.Enums;

namespace MembershipService.API.Entities
{
    public class EntitlementRule : AuditableEntity
    {
        public Guid Id { get; set; }
        public Guid NextUServiceId { get; set; }
        public NextUService NextUService { get; set; }

        public decimal Price { get; set; }
        public int CreditAmount { get; set; }
        public PeriodType Period { get; set; }
        public int? LimitPerWeek { get; set; }
        public string? Note { get; set; }
    }
}
