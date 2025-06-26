using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Enums;
using static MembershipService.API.Dtos.Request.CreateNextUServiceRequest;

namespace MembershipService.API.Services.Interfaces
{
    public interface INextUServiceService
    {
        Task<NextUServiceResponseDto> CreateAsync(CreateNextUServiceRequest request);
        Task<NextUServiceResponseDto> GetByIdAsync(Guid id);
        Task<List<NextUServiceResponseDto>> GetAllAsync();
        Task<NextUServiceResponseDto> UpdateAsync(Guid id, UpdateNextUServiceRequest request);
        Task<bool> DeleteAsync(Guid id);
        Task<List<NextUServiceResponseDto>> GetByServiceTypeAsync(ServiceType type);
        Task<List<NextUServiceResponseDto>> GetByBasicPlanIdAsync(Guid basicPlanId);
    }
}
