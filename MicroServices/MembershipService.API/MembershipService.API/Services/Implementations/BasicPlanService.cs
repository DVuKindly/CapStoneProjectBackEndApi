using AutoMapper;
using MembershipService.API.Data;
using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Entities;
using MembershipService.API.Repositories.Interfaces;
using MembershipService.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Entity = MembershipService.API.Entities.BasicPlanService;

namespace MembershipService.API.Services.Implementations
{
    public class BasicPlanService : IBasicPlanService
    {
        private readonly IBasicPlanRepository _basicPlanRepo;
        private readonly IBasicPlanServiceRepository _basicPlanServiceRepo;
        private readonly IComboPlanDurationRepository _durationRepo;
        private readonly IMapper _mapper;
        private readonly MembershipDbContext _context;

        public BasicPlanService(IBasicPlanRepository basicPlanRepo, IBasicPlanServiceRepository basicPlanServiceRepo, IComboPlanDurationRepository durationRepo, IMapper mapper, MembershipDbContext context)
        {
            _basicPlanRepo = basicPlanRepo;
            _basicPlanServiceRepo = basicPlanServiceRepo;
            _durationRepo = durationRepo;
            _mapper = mapper;
            _context = context;
        }

        public async Task<BasicPlanResponseDto> CreateAsync(CreateBasicPlanRequest request)
        {
            var entity = _mapper.Map<BasicPlan>(request);
            entity.Id = Guid.NewGuid();

            // Create BasicPlan
            var result = await _basicPlanRepo.AddAsync(entity);

            // Add BasicPlanServices
            var serviceEntities = request.ServiceIds.Select(sid => new Entity
            {
                BasicPlanId = result.Id,
                NextUServiceId = sid
            }).ToList();

            await _basicPlanServiceRepo.AddRangeAsync(serviceEntities);

            // Add Durations
            var durationEntities = request.PackageDurations.Select(pd => new ComboPlanDuration
            {
                Id = Guid.NewGuid(),
                BasicPlanId = result.Id,
                ComboPlanId = null,
                PackageDurationId = pd.DurationId,
                DiscountDurationRate = pd.DiscountRate
            }).ToList();

            await _durationRepo.AddRangeAsync(durationEntities);

            return await GetByIdAsync(result.Id);
        }

        public async Task<BasicPlanResponseDto> UpdateAsync(Guid id, UpdateBasicPlanRequest request)
        {
            var entity = await _basicPlanRepo.GetByIdAsync(id);
            if (entity == null) throw new Exception("BasicPlan not found");

            _mapper.Map(request, entity);
            await _basicPlanRepo.UpdateAsync(entity);

            // Update BasicPlanServices
            var existingServices = await _basicPlanServiceRepo.GetByPackageIdAsync(id);
            var existingServiceIds = existingServices.Select(s => s.NextUServiceId).ToList();
            var newServiceIds = request.ServiceIds;

            var toAdd = newServiceIds.Except(existingServiceIds).ToList();
            var toRemove = existingServiceIds.Except(newServiceIds).ToList();

            if (toRemove.Any())
            {
                var removeEntities = existingServices.Where(x => toRemove.Contains(x.NextUServiceId)).ToList();
                _context.BasicPlanServices.RemoveRange(removeEntities); // nếu cần xử lý logic nội bộ thì move vào repo
            }

            var addEntities = toAdd.Select(sid => new Entity
            {
                BasicPlanId = id,
                NextUServiceId = sid
            });

            await _basicPlanServiceRepo.AddRangeAsync(addEntities);

            // Update Durations (xóa hết rồi thêm lại)
            await _durationRepo.RemoveByBasicPlanIdAsync(id);

            var newDurations = request.PackageDurations.Select(pd => new ComboPlanDuration
            {
                Id = Guid.NewGuid(),
                BasicPlanId = id,
                PackageDurationId = pd.DurationId,
                DiscountDurationRate = pd.DiscountRate
            }).ToList();

            await _durationRepo.AddRangeAsync(newDurations);

            return await GetByIdAsync(id);
        }

        public async Task<BasicPlanResponseDto> GetByIdAsync(Guid id)
        {
            var entity = await _basicPlanRepo.GetByIdAsync(id);
            if (entity == null) return null;

            var dto = _mapper.Map<BasicPlanResponseDto>(entity);
            dto.ServiceIds = entity.BasicPlanServices?.Select(x => x.NextUServiceId).ToList() ?? new();
            dto.PackageDurations = entity.ComboPlanDurations?.Select(x => new PackageDurationDto
            {
                DurationId = x.PackageDurationId,
                DiscountRate = x.DiscountDurationRate
            }).ToList() ?? new();

            return dto;
        }

        public async Task<List<BasicPlanResponseDto>> GetAllAsync()
        {
            var list = await _basicPlanRepo.GetAllAsync();
            return list.Select(entity => new BasicPlanResponseDto
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,
                Description = entity.Description,
                Price = entity.Price,
                VerifyBuy = entity.VerifyBuy,
                PlanCategoryId = entity.PlanCategoryId,
                LocationId = entity.LocationId,
                ServiceIds = entity.BasicPlanServices?.Select(x => x.NextUServiceId).ToList() ?? new(),
                PackageDurations = entity.ComboPlanDurations?.Select(x => new PackageDurationDto
                {
                    DurationId = x.PackageDurationId,
                    DiscountRate = x.DiscountDurationRate
                }).ToList() ?? new()
            }).ToList();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _basicPlanRepo.DeleteAsync(id);
        }
    }
}
