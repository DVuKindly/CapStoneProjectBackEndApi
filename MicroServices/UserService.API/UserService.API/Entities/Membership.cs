using System.ComponentModel.DataAnnotations;


namespace UserService.API.Entities
{
    public class Membership 
    {
        public Guid Id { get; set; }

        [Required]
        public Guid AccountId { get; set; }

        [Required]
        public Guid PackageId { get; set; }

        [Required]
        public string PackageType { get; set; } = "basic"; 

        [Required]
        [MaxLength(200)]
        public string PackageName { get; set; }

        public decimal Amount { get; set; }

        [Required]
        public Guid LocationId { get; set; }

        public bool UsedForRoleUpgrade { get; set; } = false;

        public DateTime PurchasedAt { get; set; } = DateTime.UtcNow;

     
        public UserProfile? UserProfile { get; set; }
    }
}
