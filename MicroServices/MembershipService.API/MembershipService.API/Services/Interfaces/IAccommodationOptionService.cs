using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;

namespace MembershipService.API.Services.Interfaces
{
    public interface IAccommodationOptionService
    {
        Task<List<AccommodationOptionResponse>> GetAllAsync();
        Task<AccommodationOptionResponse?> GetByIdAsync(Guid id);
        Task<AccommodationOptionResponse> CreateAsync(CreateAccommodationOptionRequest request);
        Task<AccommodationOptionResponse> UpdateAsync(Guid id, UpdateAccommodationOptionRequest request);
        Task<bool> DeleteAsync(Guid id);
    }
}
