using MembershipService.API.Data;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Entities;
using MembershipService.API.Enums;
using MembershipService.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MembershipService.API.Repositories.Implementations
{
    public class BasicPlanRepository : IBasicPlanRepository
    {
        private readonly MembershipDbContext _context;

        public BasicPlanRepository(MembershipDbContext context)
        {
            _context = context;
        }

        public async Task<BasicPlan> AddAsync(BasicPlan entity)
        {
            _context.BasicPlans.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<BasicPlanResponseDto>> GetByTypeIdAsync(Guid typeId)
        {
            return await _context.BasicPlans
                .Where(bp => bp.BasicPlanTypeId == typeId)
                .AsNoTracking()
                .Select(bp => new BasicPlanResponseDto
                {
                    Id = bp.Id,
                    Code = bp.Code,
                    Name = bp.Name,
                    Description = bp.Description,
                    Price = bp.Price,
                    VerifyBuy = bp.VerifyBuy,
                    BasicPlanTypeId = bp.BasicPlanTypeId,
                    BasicPlanType = bp.BasicPlanType.Name,
                    BasicPlanTypeCode = bp.BasicPlanType.Code,

                    PlanCategoryId = bp.BasicPlanCategoryId,
                    PlanCategoryName = bp.BasicPlanCategory.Name,
                    PlanLevelId = bp.PlanLevelId,
                    PlanLevelName = bp.BasicPlanLevel.Name,
                    TargetAudienceId = bp.TargetAudienceId,
                    TargetAudienceName = bp.PlanTargetAudience.Name,
                    LocationId = bp.LocationId,
                    LocationName = bp.Location != null ? bp.Location.Name : null,

                    PlanDurations = bp.ComboPlanDurations.Select(cd => new PlanDurationResponseDto
                    {
                        PlanDurationId = cd.PackageDurationId,
                        DiscountRate = cd.DiscountDurationRate,
                        PlanDurationUnit = cd.PackageDuration.Unit.ToString(),
                        PlanDurationValue = cd.PackageDuration.Value.ToString(),
                        PlanDurationDescription = cd.PackageDuration.Description
                    }).ToList(),

                    Acomodations = bp.BasicPlanType.Code == "Accomodation"
                        ? bp.BasicPlanRooms.Select(r => new BasicPlanRoomResponseDto
                        {
                            AccomodationId = r.AccommodationOption.Id,
                            AccomodationDescription = r.AccommodationOption.Description,
                            RoomType = r.AccommodationOption.RoomType.Name
                        }).ToList()
                        : new List<BasicPlanRoomResponseDto>(),

                    Entitlements = bp.BasicPlanType.Code != "Accomodation"
                        ? bp.BasicPlanEntitlements.Select(e => new EntitlementResponseDto
                        {
                            EntitlementId = e.EntitlementRule.Id,
                            NextUSerName = e.EntitlementRule.NextUService.Name
                        }).ToList()
                        : new List<EntitlementResponseDto>(),

                    PlanSource = "basic"
                })
                .ToListAsync();
        }

        public async Task<List<BasicPlanResponseDto>> GetAllAsync()
        {
            return await _context.BasicPlans
                .AsNoTracking()
                .Select(bp => new BasicPlanResponseDto
                {
                    Id = bp.Id,
                    Code = bp.Code,
                    Name = bp.Name,
                    Description = bp.Description,
                    Price = bp.Price,
                    VerifyBuy = bp.VerifyBuy,
                    BasicPlanTypeId = bp.BasicPlanTypeId,
                    BasicPlanType = bp.BasicPlanType.Name,
                    BasicPlanTypeCode = bp.BasicPlanType.Code,

                    PlanCategoryId = bp.BasicPlanCategoryId,
                    PlanCategoryName = bp.BasicPlanCategory.Name,
                    PlanLevelId = bp.PlanLevelId,
                    PlanLevelName = bp.BasicPlanLevel.Name,
                    TargetAudienceId = bp.TargetAudienceId,
                    TargetAudienceName = bp.PlanTargetAudience.Name,

                    LocationId = bp.LocationId,
                    LocationName = bp.Location != null ? bp.Location.Name : null,

                    PlanDurations = bp.ComboPlanDurations.Select(cd => new PlanDurationResponseDto
                    {
                        PlanDurationId = cd.PackageDurationId,
                        DiscountRate = cd.DiscountDurationRate,
                        PlanDurationUnit = cd.PackageDuration.Unit.ToString(),
                        PlanDurationValue = cd.PackageDuration.Value.ToString(),
                        PlanDurationDescription = cd.PackageDuration.Description
                    }).ToList(),

                    Acomodations = bp.BasicPlanType.Code == "Accommodation"
                        ? bp.BasicPlanRooms.Select(r => new BasicPlanRoomResponseDto
                        {
                            AccomodationId = r.AccommodationOption.Id,
                            AccomodationDescription = r.AccommodationOption.Description,
                            RoomType = r.AccommodationOption.RoomType.Name
                        }).ToList()
                        : new List<BasicPlanRoomResponseDto>(),

                    Entitlements = bp.BasicPlanType.Code != "Accommodation"
                        ? bp.BasicPlanEntitlements.Select(e => new EntitlementResponseDto
                        {
                            EntitlementId = e.EntitlementRule.Id,
                            NextUSerName = e.EntitlementRule.NextUService.Name
                        }).ToList()
                        : new List<EntitlementResponseDto>(),

                    PlanSource = "basic"
                })
                .ToListAsync();
        }


        public async Task<BasicPlan?> GetByIdAsync(Guid id)
        {
            return await _context.BasicPlans
                .Include(x => x.ComboPlanDurations)
                    .ThenInclude(d => d.PackageDuration)
                .Include(x => x.BasicPlanRooms)
                    .ThenInclude(a => a.AccommodationOption)
                    .ThenInclude(rt => rt.RoomType)
                .Include(x => x.BasicPlanEntitlements)
                    .ThenInclude(e => e.EntitlementRule)
                    .ThenInclude(er => er.NextUService)
                .Include(x => x.Location)
                .Include(x => x.BasicPlanType)
                .Include(x => x.BasicPlanCategory)
                .Include(x => x.BasicPlanLevel)
                .Include(x => x.PlanTargetAudience)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BasicPlan> UpdateAsync(BasicPlan entity)
        {
            _context.BasicPlans.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.BasicPlans.FindAsync(id);
            if (entity == null) return false;

            _context.BasicPlans.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }



    }
}
