using MembershipService.API.Enums;

namespace MembershipService.API.Dtos.Request
{
    public class MediaUploadRequestDto
    {
        public IFormFile File { get; set; } = null!;
        public ActorType ActorType { get; set; }
        public Guid ActorId { get; set; }
        public string? Description { get; set; }
        public string Type { get; set; } = "image";
    }
}