using MembershipService.API.Dtos.Response;

namespace MembershipService.API.Services.Interfaces
{
    public interface IPlanLevelService
    {
        Task<List<PlanLevelDto>> GetAllAsync();
    }
}