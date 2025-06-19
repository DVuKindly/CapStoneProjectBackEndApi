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

        public int PackageDurationId { get; set; }
        public PackageDuration PackageDuration { get; set; }
        public Guid? LocationId { get; set; }
        public Location? Location { get; set; } = null!;

        public ICollection<Media> MediaGallery { get; set; }
        public ICollection<ComboPlan> ComboPlans { get; set; }
        public ICollection<BasicPlanService> BasicPlanServices { get; set; }
    }
}
