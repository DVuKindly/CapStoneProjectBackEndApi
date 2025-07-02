using MembershipService.API.Entities.Common;

namespace MembershipService.API.Entities
{
    public class Location : AuditableEntity
    {
        public Guid Id { get; set; }              
        public string Code { get; set; } = null!; 
        public string Name { get; set; } = null!; 
        public string? Description { get; set; }

        
        public ICollection<BasicPlan>? BasicPlans { get; set; }
        public ICollection<ComboPlan>? ComboPlans { get; set; }
        public ICollection<NextUService>? NextUServices { get; set; }
        public ICollection<AccommodationOption> AccommodationOptions { get; set; }
    }
}
