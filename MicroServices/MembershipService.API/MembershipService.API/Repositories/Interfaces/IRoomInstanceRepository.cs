using MembershipService.API.Entities;

namespace MembershipService.API.Repositories.Interfaces
{
    public interface IRoomInstanceRepository
    {
        Task<List<RoomInstance>> GetByAccommodationOptionIdAsync(Guid optionId);
        Task<List<RoomInstance>> GetAllAsync();
        //Task<List<RoomInstance>> GetByBasicPlanIdAsync(Guid planId);
        Task<List<RoomInstance>> GetByLocationIdAsync(Guid locationId);
        Task<RoomInstance?> GetByIdAsync(Guid id);
        Task<RoomInstance> CreateAsync(RoomInstance entity);
        Task<RoomInstance> UpdateAsync(RoomInstance entity);
        Task<bool> DeleteAsync(Guid id);
    }
}
