using AutoMapper;
using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Entities;
using MembershipService.API.Repositories.Interfaces;
using MembershipService.API.Services.Interfaces;
using Entity = MembershipService.API.Entities.ComboPlanService;
namespace MembershipService.API.Services.Implementations
{
    public class ComboPlanService : IComboPlanService
    {
        private readonly IComboPlanRepository _comboRepo;
        private readonly IComboPlanServiceRepository _serviceRepo;
        private readonly IMapper _mapper;

        public ComboPlanService(IComboPlanRepository comboRepo, IComboPlanServiceRepository serviceRepo, IMapper mapper)
        {
            _comboRepo = comboRepo;
            _serviceRepo = serviceRepo;
            _mapper = mapper;
        }

        public async Task<ComboPlanResponse> CreateAsync(ComboPlanCreateRequest request)
        {
            var entity = _mapper.Map<ComboPlan>(request);

            var createdComboPlan = await _comboRepo.CreateAsync(entity);

            var services = request.NextUServiceIds.Select(serviceId => new Entity
            {
                ComboPlanId = createdComboPlan.Id,
                NextUServiceId = serviceId
            }).ToList();

            await _serviceRepo.AddRangeAsync(services);

            // Gắn lại danh sách dịch vụ vào entity để ánh xạ phản hồi chính xác
            createdComboPlan.ComboPlanServices = services;

            // Load thêm liên kết (PackageDuration, Location) nếu cần
            createdComboPlan.BasicPlan = entity.BasicPlan;
            createdComboPlan.PackageLevel = entity.PackageLevel;
            createdComboPlan.Location = entity.Location;

            // Ánh xạ sang DTO phản hồi
            var result = _mapper.Map<ComboPlanResponse>(createdComboPlan);
            result.NextUServiceIds = request.NextUServiceIds;

            return result;

        }

        public async Task<List<ComboPlanResponse>> GetAllAsync()
        {
            var comboPlans = await _comboRepo.GetAllAsync();
            return comboPlans.Select(cbp => new ComboPlanResponse
            {
                Id = cbp.Id,
                Code = cbp.Code,
                Name = cbp.Name,
                TotalPrice = cbp.TotalPrice,
                DiscountRate = cbp.DiscountRate,
                PackageLevelId = cbp.PackageLevelId,
                PackageLevelName = cbp.PackageLevel?.Name,
                LocationId = cbp.LocationId ?? Guid.Empty,
                LocationName = cbp.Location?.Name,
                BasicPlanId = cbp.BasicPlanId,
                BasicPlanName = cbp.BasicPlan?.Name,
                NextUServiceIds = cbp.ComboPlanServices.Select(x => x.NextUServiceId).ToList()
            }).ToList();
        }

        public async Task<ComboPlanResponse?> GetByIdAsync(Guid id)
        {
            var entity = await _comboRepo.GetByIdAsync(id);
            if (entity == null) return null;

            return new ComboPlanResponse
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,
                TotalPrice = entity.TotalPrice,
                DiscountRate = entity.DiscountRate,
                PackageLevelId = entity.PackageLevelId,
                PackageLevelName = entity.PackageLevel?.Name,
                LocationId = entity.LocationId ?? Guid.Empty,
                LocationName = entity.Location?.Name,
                BasicPlanId = entity.BasicPlanId,
                BasicPlanName = entity.BasicPlan?.Name,
                NextUServiceIds = entity.ComboPlanServices.Select(x => x.NextUServiceId).ToList()
            };
        }

        public async Task<bool> UpdateAsync(Guid id, ComboPlanUpdateRequest request)
        {
            var entity = await _comboRepo.GetByIdAsync(id);
            if (entity == null) return false;

            _mapper.Map(request, entity);
            await _comboRepo.UpdateAsync(entity);

            await _serviceRepo.DeleteByPackageIdAsync(id);
            var newServices = request.NextUServiceIds.Select(x => new Entity
            {
                ComboPlanId = id,
                NextUServiceId = x
            });
            await _serviceRepo.AddRangeAsync(newServices);

            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _comboRepo.GetByIdAsync(id);
            if (entity == null) return false;

            await _serviceRepo.DeleteByPackageIdAsync(id);
            await _comboRepo.DeleteAsync(entity);
            return true;
        }
    }
}
