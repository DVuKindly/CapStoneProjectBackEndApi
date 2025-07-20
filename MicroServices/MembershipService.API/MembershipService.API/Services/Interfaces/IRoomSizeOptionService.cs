using MembershipService.API.Dtos.Response;
using MembershipService.API.Entities;

namespace MembershipService.API.Services.Interfaces
{
    public interface IRoomSizeOptionService
    {
        Task<List<SimpleOptionRoomDto>> GetAllAsync();
    }
}
