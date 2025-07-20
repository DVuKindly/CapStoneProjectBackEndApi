using MembershipService.API.Data;
using MembershipService.API.Entities;
using MembershipService.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MembershipService.API.Repositories.Implementations
{
    public class BedTypeOptionRepository : IBedTypeOptionRepository
    {
        private readonly MembershipDbContext _db;

        public BedTypeOptionRepository(MembershipDbContext db)
        {
            _db = db;
        }

        public async Task<List<BedTypeOption>> GetAllAsync()
        {
            return await _db.BedTypeOptions
                .OrderBy(x => x.Id)
                .ToListAsync();
        }
    }

}
