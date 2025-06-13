using AutoMapper;
using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Entities;
using MembershipService.API.Repositories.Interfaces;
using MembershipService.API.Services.Interfaces;
using Entity = MembershipService.API.Entities.BasicPackageService;

namespace MembershipService.API.Services.Implementations
{
    public class BasicPackageService : IBasicPackageService
    {
        private readonly IBasicPackageRepository _packageRepo;
        private readonly IBasicPackageServiceRepository _serviceRepo;
        private readonly IMapper _mapper;

        public BasicPackageService(
            IBasicPackageRepository packageRepo,
            IBasicPackageServiceRepository serviceRepo,
            IMapper mapper)
        {
            _packageRepo = packageRepo;
            _serviceRepo = serviceRepo;
            _mapper = mapper;
        }

        public async Task<BasicPackageResponse> CreateAsync(BasicPackageCreateRequest request)
        {
            var entity = _mapper.Map<BasicPackage>(request);
            var createdPackage = await _packageRepo.CreateAsync(entity);

            var services = request.NextUServiceIds.Select(id => new Entity
            {
                BasicPackageId = createdPackage.Id,
                NextUServiceId = id
            });

            await _serviceRepo.AddRangeAsync(services);

            var result = _mapper.Map<BasicPackageResponse>(createdPackage);
            result.NextUServiceIds = request.NextUServiceIds;
            return result;
        }

        public async Task<BasicPackageResponse?> GetByIdAsync(Guid id)
        {
            var entity = await _packageRepo.GetByIdAsync(id);
            if (entity == null) return null;

            return new BasicPackageResponse
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,
                Description = entity.Description,
                Price = entity.Price,
                VerifyBuy = entity.VerifyBuy,
                PackageLevelId = entity.PackageLevelId,
                PackageLevelName = entity.PackageLevel?.Name,
                PackageDurationId = entity.PackageDurationId,
                PackageDurationName = entity.PackageDuration?.Description,
                NextUServiceIds = entity.BasicPackageServices.Select(x => x.NextUServiceId).ToList()
            };
        }

        public async Task<List<BasicPackageResponse>> GetAllAsync()
        {
            var packages = await _packageRepo.GetAllAsync();
            return packages.Select(pkg => new BasicPackageResponse
            {
                Id = pkg.Id,
                Code = pkg.Code,
                Name = pkg.Name,
                Description = pkg.Description,
                Price = pkg.Price,
                VerifyBuy = pkg.VerifyBuy,
                PackageLevelId = pkg.PackageLevelId,
                PackageLevelName = pkg.PackageLevel?.Name,
                PackageDurationId = pkg.PackageDurationId,
                PackageDurationName = pkg.PackageDuration?.Description,
                NextUServiceIds = pkg.BasicPackageServices.Select(x => x.NextUServiceId).ToList()
            }).ToList();
        }

        public async Task<bool> UpdateAsync(Guid id, BasicPackageUpdateRequest request)
        {
            var entity = await _packageRepo.GetByIdAsync(id);
            if (entity == null) return false;

            _mapper.Map(request, entity);
            await _packageRepo.UpdateAsync(entity);

            await _serviceRepo.DeleteByPackageIdAsync(id);
            var newServices = request.NextUServiceIds.Select(x => new Entities.BasicPackageService
            {
                BasicPackageId = id,
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
