using MembershipService.API.Entities;

namespace MembershipService.API.Repositories.Interfaces
{
    public interface IRoomFloorOptionRepository
    {
        Task<List<RoomFloorOption>> GetAllAsync();
    }

}
