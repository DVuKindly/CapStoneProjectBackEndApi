using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Entities;

namespace MembershipService.API.Repositories.Interfaces
{
    public interface IAccommodationOptionRepository
    {
        Task<List<AccommodationOption>> GetAllAsync();
        Task<AccommodationOption?> GetByIdAsync(Guid id);
        Task<AccommodationOption> CreateAsync(AccommodationOption option);
        Task<AccommodationOption> UpdateAsync(AccommodationOption option);
        Task<bool> DeleteAsync(Guid id);
    }
}
