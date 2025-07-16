using MembershipService.API.Enums;

namespace MembershipService.API.Dtos.Request
{
    public class CreateEntitlementRuleDto
    {
        public Guid NextUServiceId { get; set; }
        public decimal Price { get; set; }
        public int CreditAmount { get; set; }
        public PeriodType Period { get; set; }
        public int? LimitPerPeriod { get; set; }
        public string? Note { get; set; }
    }

    public class UpdateEntitlementRuleDto : CreateEntitlementRuleDto
    {
        public Guid Id { get; set; }
    }
}
