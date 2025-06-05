using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.API.Entities
{
    public class StaffProfile
    {
        public Guid Id { get; set; }

        [Required, MaxLength(100)]
        public string AccountId { get; set; } = null!;

        [ForeignKey(nameof(AccountId))]
        public UserProfile? UserProfile { get; set; }

        [MaxLength(100)]
        public string? StaffGroup { get; set; }
    }
}
