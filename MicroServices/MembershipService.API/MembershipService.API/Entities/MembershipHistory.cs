using MembershipService.API.Entities.Common;

namespace MembershipService.API.Entities
{
    public class MembershipHistory : AuditableEntity
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid? BasicPlanId { get; set; }
        public BasicPlan? BasicPlan { get; set; }

        public Guid? ComboPlanId { get; set; }
        public ComboPlan? ComboPlan { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Guid? PaymentId { get; set; }

        public string PackageNameSnapshot { get; set; } = null!;
        public decimal PriceSnapshot { get; set; }

        public DateTime PurchasedAt { get; set; }
        public string Note { get; set; } = ""; // "Expired", "Cancelled", "Upgraded"
    }
}
