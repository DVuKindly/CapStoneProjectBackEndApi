using MembershipService.API.Data;
using MembershipService.API.Entities;
using MembershipService.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MembershipService.API.Repositories.Implementations
{
    public class EntitlementRuleRepository : IEntitlementRuleRepository
    {
        private readonly MembershipDbContext _context;

        public EntitlementRuleRepository(MembershipDbContext context)
        {
            _context = context;
        }

        public async Task<EntitlementRule> GetByIdAsync(Guid id)
        {
            return await _context.EntitlementRules
                .Include(e => e.NextUService)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<List<EntitlementRule>> GetAllAsync()
        {
            return await _context.EntitlementRules
                .Include(e => e.NextUService)
                .ToListAsync();
        }


        public async Task<EntitlementRule> AddAsync(EntitlementRule entity)
        {
            _context.EntitlementRules.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(EntitlementRule entity)
        {
            _context.EntitlementRules.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return false;
            _context.EntitlementRules.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<EntitlementRule>> GetByBasicPlanIdAsync(Guid basicPlanId)
        {
            // Lấy danh sách EntitlementRuleId từ bảng trung gian BasicPlanEntitlements
            var ruleIds = await _context.BasicPlanEntitlements
                .Where(bpe => bpe.BasicPlanId == basicPlanId)
                .Select(bpe => bpe.EntitlementRuleId)
                .ToListAsync();

            // Truy vấn các EntitlementRule có Id nằm trong danh sách trên
            return await _context.EntitlementRules
                .Where(r => ruleIds.Contains(r.Id))
                .Include(r => r.NextUService) // ✅ Include đúng tên navigation
                    .ThenInclude(svc => svc.Property)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalEntitlementPriceAsync(Guid basicPlanId)
        {
            return await _context.BasicPlanEntitlements
                .Where(bpe => bpe.BasicPlanId == basicPlanId)
                .Select(bpe => bpe.EntitlementRule.Price)
                .SumAsync();
        }



    }
}
