using MembershipService.API.Entities;

namespace MembershipService.API.Repositories.Interfaces
{
    public interface IBedTypeOptionRepository
    {
        Task<List<BedTypeOption>> GetAllAsync();
    }

}
