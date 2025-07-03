namespace MembershipService.API.Entities
{
    public class BasicPlanEntitlement
    {
        public Guid Id { get; set; }
        public Guid BasicPlanId { get; set; }
        public BasicPlan BasicPlan { get; set; }

        public Guid EntitlementRuleId { get; set; }
        public EntitlementRule EntitlementRule { get; set; }

        public int? Quantity { get; set; } // Số lượt/hạn mức cụ thể
    }
}
