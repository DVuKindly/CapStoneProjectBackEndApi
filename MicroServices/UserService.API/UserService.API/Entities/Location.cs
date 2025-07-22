using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Locations")]
public class Location
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? Description { get; set; }

    public Guid CityId { get; set; }

    [ForeignKey("CityId")]
    public City City { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // 🔁 Navigation
    public ICollection<Property> Properties { get; set; } = new List<Property>();
}
