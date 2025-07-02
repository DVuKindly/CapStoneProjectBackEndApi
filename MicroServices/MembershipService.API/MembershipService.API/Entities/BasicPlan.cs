using MembershipService.API.Entities.Common;

namespace MembershipService.API.Entities
{
    public class BasicPlan : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public bool VerifyBuy { get; set; }

        public Guid BasicPlanTypeId { get; set; }
        public BasicPlanType BasicPlanType { get; set; }

        public int BasicPlanCategoryId { get; set; }
        public BasicPlanCategory BasicPlanCategory { get; set; }

        public int PlanLevelId { get; set; }
        public BasicPlanLevel BasicPlanLevel { get; set; }

        public int TargetAudienceId { get; set; }
        public PlanTargetAudience PlanTargetAudience { get; set; }

        public Guid? LocationId { get; set; }
        public Location? Location { get; set; } = null!;

        public ICollection<BasicPlanEntitlement> BasicPlanEntitlements { get; set; }
        public ICollection<BasicPlanRoom> BasicPlanRooms { get; set; }
        public ICollection<ComboPlanBasic> ComboPlanBasics { get; set; }
        public ICollection<Membership> Memberships { get; set; }
        public ICollection<MembershipHistory> MembershipHistories { get; set; }
        public ICollection<ComboPlanDuration> ComboPlanDurations { get; set; }


    }
}
