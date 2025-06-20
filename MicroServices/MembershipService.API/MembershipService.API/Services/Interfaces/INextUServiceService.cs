using MembershipService.API.Dtos.Response;
using static MembershipService.API.Dtos.Request.NextUServiceRequestDto;

namespace MembershipService.API.Services.Interfaces
{
    public interface INextUServiceService
    {
        Task<NextUServiceResponseDto> CreateAsync(CreateNextUServiceRequest request);
        Task<NextUServiceResponseDto> GetByIdAsync(Guid id);
        Task<List<NextUServiceResponseDto>> GetAllAsync();
        Task<NextUServiceResponseDto> UpdateAsync(Guid id, UpdateNextUServiceRequest request);
        Task<bool> DeleteAsync(Guid id);
    }
}
