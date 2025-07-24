using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Enums;

namespace MembershipService.API.Services.Interfaces
{
    public interface IMediaService
    {
        Task<MediaResponseDto> UploadAsync(MediaUploadRequestDto request);
        Task<List<MediaResponseDto>> GetByActorAsync(ActorType actorType, Guid actorId);
        Task<Dictionary<Guid, List<MediaResponseDto>>> GetGroupedByActorAsync(ActorType actorType, List<Guid> actorIds);
    }
}