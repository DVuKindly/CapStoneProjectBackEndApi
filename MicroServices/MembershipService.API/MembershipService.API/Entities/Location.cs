using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MembershipService.API.Entities
{
    [Table("Locations")]
    public class Location
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public Guid CityId { get; set; }

        [ForeignKey("CityId")]
        public City City { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // 🔁 Navigation
        public ICollection<Property> Properties { get; set; } = new List<Property>();
    }
}
