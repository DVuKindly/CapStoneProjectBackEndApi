using MembershipService.API.Entities;

namespace MembershipService.API.Repositories.Interfaces
{
    public interface IRoomViewOptionRepository
    {
        Task<List<RoomViewOption>> GetAllAsync();
    }

}
