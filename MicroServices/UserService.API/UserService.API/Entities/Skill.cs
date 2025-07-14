using System;
using System.ComponentModel.DataAnnotations;

namespace UserService.API.Entities
{
    public class Skill
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;
    }
}
