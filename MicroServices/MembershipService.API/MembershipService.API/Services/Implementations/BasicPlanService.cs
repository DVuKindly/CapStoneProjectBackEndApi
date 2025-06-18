using AutoMapper;
using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Entities;
using MembershipService.API.Repositories.Interfaces;
using MembershipService.API.Services.Interfaces;
using Entity = MembershipService.API.Entities.BasicPlanService;

namespace MembershipService.API.Services.Implementations
{
    public class BasicPlanService : IBasicPlanService
    {
        private readonly IBasicPlanRepository _packageRepo;
        private readonly IBasicPlanServiceRepository _serviceRepo;
        private readonly IMapper _mapper;

        public BasicPlanService(
            IBasicPlanRepository packageRepo,
            IBasicPlanServiceRepository serviceRepo,
            IMapper mapper)
        {
            _packageRepo = packageRepo;
            _serviceRepo = serviceRepo;
            _mapper = mapper;
        }

        public async Task<BasicPlanResponse> CreateAsync(BasicPlanCreateRequest request)
        {
            // Ánh xạ DTO sang Entity
            var entity = _mapper.Map<BasicPlan>(request);

            // Tạo bản ghi BasicPlan
            var createdPackage = await _packageRepo.CreateAsync(entity);

            // Tạo các bản ghi liên kết dịch vụ
            var services = request.NextUServiceIds.Select(serviceId => new Entity
            {
                BasicPlanId = createdPackage.Id,
                NextUServiceId = serviceId
            }).ToList();

            await _serviceRepo.AddRangeAsync(services);

            // Gắn lại danh sách dịch vụ vào entity để ánh xạ phản hồi chính xác
            createdPackage.BasicPlanServices = services;

            // Load thêm liên kết (PackageDuration, Location) nếu cần
            createdPackage.PackageDuration = entity.PackageDuration;
            createdPackage.Location = entity.Location;

            // Ánh xạ sang DTO phản hồi
            var result = _mapper.Map<BasicPlanResponse>(createdPackage);
            result.NextUServiceIds = request.NextUServiceIds;

            return result;
        }


        public async Task<BasicPlanResponse?> GetByIdAsync(Guid id)
        {
            var entity = await _packageRepo.GetByIdAsync(id);
            if (entity == null) return null;

            return new BasicPlanResponse
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,
                Description = entity.Description,
                Price = entity.Price,
                VerifyBuy = entity.VerifyBuy,
                PackageDurationId = entity.PackageDurationId,
                PackageDurationName = entity.PackageDuration?.Description,
                LocationId = entity.LocationId ?? Guid.Empty,
                LocationName = entity.Location?.Name,
                NextUServiceIds = entity.BasicPlanServices.Select(x => x.NextUServiceId).ToList()
            };
        }

        public async Task<List<BasicPlanResponse>> GetAllAsync()
        {
            var packages = await _packageRepo.GetAllAsync();
            return packages.Select(pkg => new BasicPlanResponse
            {
                Id = pkg.Id,
                Code = pkg.Code,
                Name = pkg.Name,
                Description = pkg.Description,
                Price = pkg.Price,
                VerifyBuy = pkg.VerifyBuy,
                PackageDurationId = pkg.PackageDurationId,
                PackageDurationName = pkg.PackageDuration?.Description,
                LocationId = pkg.LocationId ?? Guid.Empty,
                LocationName = pkg.Location?.Name,
                NextUServiceIds = pkg.BasicPlanServices.Select(x => x.NextUServiceId).ToList()
            }).ToList();
        }

        public async Task<bool> UpdateAsync(Guid id, BasicPackageUpdateRequest request)
        {
            var entity = await _packageRepo.GetByIdAsync(id);
            if (entity == null) return false;

            _mapper.Map(request, entity);
            await _packageRepo.UpdateAsync(entity);

            await _serviceRepo.DeleteByPackageIdAsync(id);
            var newServices = request.NextUServiceIds.Select(x => new Entity
            {
                BasicPlanId = id,
                NextUServiceId = x
            });
            await _serviceRepo.AddRangeAsync(newServices);

            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _packageRepo.GetByIdAsync(id);
            if (entity == null) return false;

            await _serviceRepo.DeleteByPackageIdAsync(id);
            await _packageRepo.DeleteAsync(entity);
            return true;
        }

    }
}
