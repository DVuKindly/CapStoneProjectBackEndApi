using MembershipService.API.Entities;

namespace MembershipService.API.Repositories.Interfaces
{
    public interface IRoomSizeOptionRepository
    {
        Task<List<RoomSizeOption>> GetAllAsync();
    }
}
