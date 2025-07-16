 using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;

namespace MembershipService.API.Services.Interfaces
{
    public interface IRoomInstanceService
    {
        Task<List<RoomInstanceResponse>> GetByAccommodationOptionIdAsync(Guid optionId);
        Task<List<RoomInstanceResponse>> GetAllAsync();
        //Task<List<RoomInstanceResponse>> GetByBasicPlanIdAsync(Guid planId);
        Task<List<RoomInstanceResponse>> GetByLocationIdAsync(Guid locationId);
        Task<RoomInstanceResponse?> GetByIdAsync(Guid id);
        Task<RoomInstanceResponse> CreateAsync(CreateRoomInstanceRequest request);
        Task<RoomInstanceResponse> UpdateAsync(Guid id, UpdateRoomInstanceRequest request);
        Task<bool> DeleteAsync(Guid id);
    }
}
