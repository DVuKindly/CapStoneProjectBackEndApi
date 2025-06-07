using BffService.API.DTOs.SUPPLIER;
using System.ComponentModel.DataAnnotations;

namespace BffService.API.DTOs.AdminCreate
{
    public class RegisterSupplierRequest
    {
        [Required]
        public string UserName { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

     
        [Required]
        public string Location { get; set; } = null!;

        public bool SkipPasswordCreation { get; set; } = true;
       

        [Required]
        public SupplierProfileInfoRequest ProfileInfo { get; set; } = null!;
    }
}
