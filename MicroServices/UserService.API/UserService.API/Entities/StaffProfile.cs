using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UserService.API.Entities;

[Table("StaffProfiles")]
public class StaffProfile
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid AccountId { get; set; }

    [MaxLength(50)]
    public string? StaffGroup { get; set; }

    [MaxLength(100)]
    public string? Department { get; set; }

    [MaxLength(50)]
    public string? Level { get; set; }

    [MaxLength(50)]
    public string? Phone { get; set; }

    [MaxLength(100)]
    public string? Email { get; set; }

    [MaxLength(255)]
    public string? Address { get; set; }

    public Guid? ManagerId { get; set; }

    [MaxLength(1000)]
    public string? Note { get; set; }

    public DateTime? JoinedDate { get; set; }

    public bool IsActive { get; set; } = true;

    public Guid? LocationId { get; set; }

    // 🔁 Navigation
    [ForeignKey("AccountId")]
    public virtual UserProfile? UserProfile { get; set; }

    [ForeignKey("LocationId")]
    public virtual Property? Property { get; set; }
}
