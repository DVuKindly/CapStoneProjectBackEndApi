using MembershipService.API.Dtos.Response;

namespace MembershipService.API.Services.Interfaces
{
    public interface IPlanCategoryService
    {
        Task<List<PlanCategoryDto>> GetAllAsync();
    }
}