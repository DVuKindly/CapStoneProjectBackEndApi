using System.ComponentModel.DataAnnotations;

namespace UserService.API.DTOs.BasePositon
{
    public class CreatePropertyBasePositionDTO
    {
        [Required]
        public Guid LocationId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Description { get; set; }
    }

    public class UpdatePropertyDto : CreatePropertyBasePositionDTO { }

    public class PropertyResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public Guid LocationId { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public Guid CityId { get; set; }
        public string CityName { get; set; } = string.Empty;
    }

}
