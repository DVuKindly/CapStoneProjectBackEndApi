using MembershipService.API.Dtos.Response;

namespace MembershipService.API.Services.Interfaces
{
    public interface IPlanTargetAudienceService
    {
        Task<List<PlanTargetAudienceDto>> GetAllAsync();
    }
}