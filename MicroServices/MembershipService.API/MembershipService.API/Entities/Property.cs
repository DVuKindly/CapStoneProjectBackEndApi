using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MembershipService.API.Entities.Common;

namespace MembershipService.API.Entities
{
    public class Property : AuditableEntity
    {
        public Guid Id { get; set; }              
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
       
        public Guid? LocationId { get; set; }

        [ForeignKey("LocationId")]
        public virtual Location? Location { get; set; }


        public ICollection<BasicPlan>? BasicPlans { get; set; }
        public ICollection<ComboPlan>? ComboPlans { get; set; }
        public ICollection<NextUService>? NextUServices { get; set; }
        public ICollection<AccommodationOption> AccommodationOptions { get; set; }
    }
}
