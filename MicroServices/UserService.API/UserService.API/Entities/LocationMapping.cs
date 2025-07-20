using System.ComponentModel.DataAnnotations;

namespace UserService.API.Entities
{
    public class LocationMapping
    {
        [Key]
        public Guid Id { get; set; }
        public string RegionName { get; set; }
        public Guid PropertyId { get; set; }      // từ UserService
        public Guid MembershipLocationId { get; set; }  // từ MembershipService

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Property Property { get; set; } = null!;
    }

}
