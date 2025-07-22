using System.ComponentModel.DataAnnotations;

namespace UserService.API.DTOs.BasePositon
{
    public class CreateCityBasePositionDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Description { get; set; }
    }

    public class UpdateCityDto : CreateCityBasePositionDTO { }

    public class CityResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
