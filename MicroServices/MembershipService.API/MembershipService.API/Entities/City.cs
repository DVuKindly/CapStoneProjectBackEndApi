using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MembershipService.API.Entities
{
    [Table("Cities")]
    public class City
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // 🔁 Navigation
        public ICollection<Location> Locations { get; set; } = new List<Location>();
    }
}
