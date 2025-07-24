using MembershipService.API.Dtos.Response;
using MembershipService.API.Enums;

namespace MembershipService.API.Repositories.Interfaces
{
    public interface IMediaRepository
    {
        Task<Dictionary<Guid, List<MediaResponseDto>>> GetGroupedByActorAsync(ActorType actorType, List<Guid> actorIds);
    }
}