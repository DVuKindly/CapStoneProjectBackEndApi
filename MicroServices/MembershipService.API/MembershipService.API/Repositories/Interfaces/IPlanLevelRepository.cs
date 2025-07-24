using MembershipService.API.Entities;

namespace MembershipService.API.Repositories.Interfaces
{
    public interface IPlanLevelRepository
    {
        Task<List<PlanLevel>> GetAllAsync();
    }
}