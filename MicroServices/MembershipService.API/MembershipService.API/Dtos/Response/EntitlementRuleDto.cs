using MembershipService.API.Enums;

namespace MembershipService.API.Dtos.Response
{
    public class EntitlementRuleDto
    {
        public Guid Id { get; set; }
        public Guid NextUServiceId { get; set; }
        public string NextUServiceName { get; set; }
        public decimal Price { get; set; }
        public int CreditAmount { get; set; }
        public PeriodType Period { get; set; }
        public int? LimitPerPeriod { get; set; }
        public string? Note { get; set; }
    }
}
