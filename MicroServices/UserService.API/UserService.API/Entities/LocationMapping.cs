using System.ComponentModel.DataAnnotations;

namespace UserService.API.Entities
{
    public class LocationMapping
    {
        [Key]
        public Guid Id { get; set; }
        public string RegionName { get; set; }
        public Guid LocationRegionId { get; set; }      // từ UserService
        public Guid MembershipLocationId { get; set; }  // từ MembershipService

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public LocationRegion LocationRegion { get; set; } = null!;
    }

}
