using MembershipService.API.Entities;

namespace MembershipService.API.Repositories.Interfaces
{
    public interface IBasicPlanRoomRepository
    {
        Task<List<BasicPlanRoom>> GetByBasicPlanIdAsync(Guid basicPlanId);
        Task<BasicPlanRoom?> GetByIdAsync(Guid id);
        Task<BasicPlanRoom> AddAsync(BasicPlanRoom room);
        Task<BasicPlanRoom> UpdateAsync(BasicPlanRoom room);
        Task<bool> DeleteAsync(Guid id);
        //Task<bool> ExistsAsync(Guid basicPlanId, Guid roomId);
    }
}
