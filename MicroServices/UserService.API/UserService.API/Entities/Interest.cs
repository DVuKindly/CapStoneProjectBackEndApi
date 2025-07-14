using System;
using System.ComponentModel.DataAnnotations;

namespace UserService.API.Entities
{
    public class Interest
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;
    }
}
