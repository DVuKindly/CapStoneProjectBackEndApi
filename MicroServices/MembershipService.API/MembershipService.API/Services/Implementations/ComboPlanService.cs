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
    public class ComboPlanService : IComboPlanService
    {
        private readonly IComboPlanRepository _planRepo;
        private readonly IComboPlanBasicRepository _basicRepo;
        private readonly IComboPlanDurationRepository _durationRepo;
        private readonly IMapper _mapper;
        private readonly MembershipDbContext _context;

        public ComboPlanService(IComboPlanRepository planRepo, IComboPlanBasicRepository basicRepo, IComboPlanDurationRepository durationRepo, IMapper mapper, MembershipDbContext context)
        {
            _planRepo = planRepo;
            _basicRepo = basicRepo;
            _durationRepo = durationRepo;
            _mapper = mapper;
            _context = context;
        }

        public async Task<ComboPlanResponseDto> CreateAsync(CreateComboPlanRequest request)
        {
            var entity = _mapper.Map<ComboPlan>(request);
            var result = await _planRepo.AddAsync(entity);

            await _basicRepo.AddRangeAsync(request.BasicPlanIds
                .Select(id => new ComboPlanBasic { ComboPlanId = result.Id, BasicPlanId = id }));

            await _durationRepo.AddRangeAsync(request.PackageDurations
                .Select(d => new ComboPlanDuration
                {
                    PackageDurationId = d.DurationId,
                    DiscountDurationRate = d.DiscountRate,
                    ComboPlanId = result.Id
                }).ToList());

            return _mapper.Map<ComboPlanResponseDto>(result);
        }

        public async Task<ComboPlanResponseDto> GetByIdAsync(Guid id)
        {
            var entity = await _planRepo.GetByIdAsync(id);
            return _mapper.Map<ComboPlanResponseDto>(entity);
        }

        public async Task<List<ComboPlanResponseDto>> GetAllAsync()
        {
            var list = await _planRepo.GetAllAsync();
            return _mapper.Map<List<ComboPlanResponseDto>>(list);
        }

        public async Task<ComboPlanResponseDto> UpdateAsync(Guid id, UpdateComboPlanRequest request)
        {
            var entity = await _planRepo.GetByIdAsync(id);
            _mapper.Map(request, entity);

            var oldBasicIds = entity.ComboPlanBasics.Select(x => x.BasicPlanId).ToHashSet();
            var newBasicIds = request.BasicPlanIds.ToHashSet();

            var toRemove = entity.ComboPlanBasics.Where(x => !newBasicIds.Contains(x.BasicPlanId)).ToList();
            var toAdd = newBasicIds.Except(oldBasicIds)
                .Select(id => new ComboPlanBasic { ComboPlanId = id, BasicPlanId = id });

            _context.ComboPlanBasics.RemoveRange(toRemove);
            await _basicRepo.AddRangeAsync(toAdd);

            await _durationRepo.RemoveByComboPlanIdAsync(id);
            await _durationRepo.AddRangeAsync(request.PackageDurations
                .Select(d => new ComboPlanDuration
                {
                    PackageDurationId = d.DurationId,
                    DiscountDurationRate = d.DiscountRate,
                    ComboPlanId = id
                }).ToList());

            var updated = await _planRepo.UpdateAsync(entity);
            return _mapper.Map<ComboPlanResponseDto>(updated);
        }


        //vũ code
        public async Task<DurationDto?> GetPlanDurationAsync(Guid planId)
        {
            var plan = await _context.ComboPlans
                .Where(p => p.Id == planId)
                .Include(p => p.ComboPlanDurations)
                    .ThenInclude(d => d.PackageDuration)
                .FirstOrDefaultAsync();

            var duration = plan?.ComboPlanDurations?
                .Where(d => d.PackageDuration != null)
                .OrderBy(d => d.PackageDuration.Value) // Ưu tiên thời hạn nhỏ nhất hoặc bạn có thể dùng logic khác
                .FirstOrDefault();

            if (duration == null || duration.PackageDuration == null)
                return null;

            return new DurationDto
            {
                Value = duration.PackageDuration.Value,
                Unit = duration.PackageDuration.Unit.ToString()
            };
        }
        public async Task<List<ComboPlanResponseDto>> GetByIdsAsync(List<Guid> ids)
        {
            var plans = await _context.ComboPlans
                .Where(p => ids.Contains(p.Id))
                .Include(p => p.Location)
                .ToListAsync();

            return _mapper.Map<List<ComboPlanResponseDto>>(plans);
        }


        public async Task<bool> DeleteAsync(Guid id) => await _planRepo.DeleteAsync(id);
    }
}
