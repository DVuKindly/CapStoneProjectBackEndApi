using MembershipService.API.Entities;

namespace MembershipService.API.Repositories.Interfaces
{
    public interface IBasicPlanTypeRepository
    {
        Task<List<BasicPlanType>> GetAllAsync();
        Task<BasicPlanType?> GetByIdAsync(Guid id);
    }
}
