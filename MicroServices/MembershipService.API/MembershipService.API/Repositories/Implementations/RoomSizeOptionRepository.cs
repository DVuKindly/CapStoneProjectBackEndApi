using MembershipService.API.Data;
using MembershipService.API.Entities;
using MembershipService.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MembershipService.API.Repositories.Implementations
{
    public class RoomSizeOptionRepository : IRoomSizeOptionRepository
    {
        private readonly MembershipDbContext _context;

        public RoomSizeOptionRepository(MembershipDbContext context)
        {
            _context = context;
        }

        public async Task<List<RoomSizeOption>> GetAllAsync()
        {
            return await _context.RoomSizeOptions
                .OrderBy(x => x.Id)
                .ToListAsync();
        }
    }

}
