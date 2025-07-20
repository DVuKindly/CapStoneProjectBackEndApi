using MembershipService.API.Dtos.Response;
using MembershipService.API.Entities;

namespace MembershipService.API.Services.Interfaces
{
    public interface IRoomFloorOptionService
    {
        Task<List<SimpleOptionRoomDto>> GetAllAsync();
    }

}
