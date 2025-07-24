using MembershipService.API.Enums;

namespace MembershipService.API.Dtos.Response
{
    public class MediaResponseDto
    {
        public Guid Id { get; set; }
        public string Url { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string? Description { get; set; }
        public ActorType ActorType { get; set; }
        public Guid ActorId { get; set; }
    }
}