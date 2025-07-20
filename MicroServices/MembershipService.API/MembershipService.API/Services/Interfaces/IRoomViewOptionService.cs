using MembershipService.API.Dtos.Response;
using MembershipService.API.Entities;

namespace MembershipService.API.Services.Interfaces
{
    public interface IRoomViewOptionService
    {
        Task<List<SimpleOptionRoomDto>> GetAllAsync();
    }

}
