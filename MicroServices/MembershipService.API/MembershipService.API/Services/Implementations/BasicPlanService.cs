using AutoMapper;
using MembershipService.API.Data;
using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Entities;
using MembershipService.API.Repositories.Interfaces;
using MembershipService.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MembershipService.API.Services.Implementations
{
    public class BasicPlanService : IBasicPlanService
    {
        private readonly IBasicPlanRepository _basicPlanRepo;
        private readonly IComboPlanDurationRepository _durationRepo;
        private readonly IMapper _mapper;
        private readonly MembershipDbContext _context;
     
 

        public BasicPlanService(IBasicPlanRepository basicPlanRepo, IComboPlanDurationRepository durationRepo, IMapper mapper, MembershipDbContext context)
        {
            _basicPlanRepo = basicPlanRepo;
            _durationRepo = durationRepo;
            _mapper = mapper;
        
            _context = context;
        }

        public Task<BasicPlanResponseDto> CreateAsync(CreateBasicPlanRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _basicPlanRepo.DeleteAsync(id);
        }

        public Task<List<BasicPlanResponseDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BasicPlanResponseDto> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }


        //vũ code
        public async Task<List<BasicPlanResponse>> GetByIdsAsync(List<Guid> ids)
        {
            if (ids == null || ids.Count == 0)
                return new List<BasicPlanResponse>();

            var plans = await _context.BasicPlans
                .Where(p => ids.Contains(p.Id))
                .Include(p => p.Location)
                .Include(p => p.ComboPlanDurations)
                    .ThenInclude(d => d.PackageDuration)
                .ToListAsync();

            var result = plans.Select(p =>
            {
                var firstDuration = p.ComboPlanDurations?
                    .Where(d => d.PackageDuration != null)
                    .OrderBy(d => d.PackageDuration.Value)
                    .FirstOrDefault();

                return new BasicPlanResponse
                {
                    Id = p.Id,
                    Code = p.Code ?? string.Empty,
                    Name = p.Name ?? string.Empty,
                    Description = p.Description ?? string.Empty,
                    Price = p.Price, // ✅ bắt buộc phải có giá trị
                    LocationId = p.LocationId ?? Guid.Empty,
                    LocationName = p.Location?.Name ?? "Không xác định",
                    PackageDurationValue = firstDuration?.PackageDuration?.Value ?? 0,
                    PackageDurationUnit = firstDuration?.PackageDuration?.Unit.ToString() ?? string.Empty,
                    DurationDescription = firstDuration?.PackageDuration?.Description ?? string.Empty,
                    PlanSource = "basic"
                };
            }).ToList();

            return result;
        }

        //vũ code 
        public async Task<DurationDto?> GetPlanDurationAsync(Guid planId)
        {
            var plan = await _context.BasicPlans
                .Where(p => p.Id == planId)
                .Include(p => p.ComboPlanDurations)
                    .ThenInclude(d => d.PackageDuration)
                .FirstOrDefaultAsync();

            var duration = plan?.ComboPlanDurations?
                .Where(d => d.PackageDuration != null)
                .OrderBy(d => d.PackageDuration.Value)
                .FirstOrDefault();

            if (duration == null || duration.PackageDuration == null)
                return null;

            return new DurationDto
            {
                Value = duration.PackageDuration.Value,
                Unit = duration.PackageDuration.Unit.ToString()
            };
        }

        public Task<BasicPlanResponseDto> UpdateAsync(Guid id, UpdateBasicPlanRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
