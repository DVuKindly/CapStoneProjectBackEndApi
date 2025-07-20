using MembershipService.API.Entities;

namespace MembershipService.API.Repositories.Interfaces
{
    public interface IRoomInstanceRepository
    {
        Task<List<RoomInstance>> GetByAccommodationOptionIdAsync(Guid optionId);
        Task<List<RoomInstance>> GetAllAsync();
        Task<List<RoomInstance>> GetByPropertyIdAsync(Guid PropertyId);
        Task<RoomInstance?> GetByIdAsync(Guid id);
        Task<RoomInstance> CreateAsync(RoomInstance entity);
        Task<bool> IsRoomCodeDuplicateAsync(Guid accommodationOptionId, string roomCode);
        Task<RoomInstance> GetByIdWithNavigationAsync(Guid id);
        Task<RoomInstance> UpdateAsync(RoomInstance entity);
        Task<bool> DeleteAsync(Guid id);
    }
}
