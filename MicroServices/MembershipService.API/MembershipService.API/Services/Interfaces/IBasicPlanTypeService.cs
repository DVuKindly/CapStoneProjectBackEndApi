using MembershipService.API.Dtos.Response;

namespace MembershipService.API.Services.Interfaces
{
    public interface IBasicPlanTypeService
    {
        Task<List<BasicPlanTypeResponseDto>> GetAllAsync();
        Task<BasicPlanTypeResponseDto?> GetByIdAsync(Guid id);
    }
}
