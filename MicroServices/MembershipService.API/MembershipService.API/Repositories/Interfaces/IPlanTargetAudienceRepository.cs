using MembershipService.API.Entities;

namespace MembershipService.API.Repositories.Interfaces
{
    public interface IPlanTargetAudienceRepository
    {
        Task<List<PlanTargetAudience>> GetAllAsync();
    }
}