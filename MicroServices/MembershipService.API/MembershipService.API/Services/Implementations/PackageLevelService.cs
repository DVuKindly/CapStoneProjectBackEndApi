using AutoMapper;
using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Entities;
using MembershipService.API.Repositories.Interfaces;
using MembershipService.API.Services.Interfaces;

namespace MembershipService.API.Services.Implementations
{
    public class PackageLevelService : IPackageLevelService
    {
        private readonly IPackageLevelRepository _repo;
        private readonly IMapper _mapper;

        public PackageLevelService(IPackageLevelRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<PackageLevelResponse>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return _mapper.Map<List<PackageLevelResponse>>(list);
        }

        public async Task<PackageLevelResponse?> GetByIdAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<PackageLevelResponse>(entity);
        }

        public async Task<PackageLevelResponse> CreateAsync(PackageLevelCreateRequest request)
        {
            var entity = _mapper.Map<PackageLevel>(request);
            var created = await _repo.CreateAsync(entity);
            return _mapper.Map<PackageLevelResponse>(created);
        }

        public async Task<bool> UpdateAsync(Guid id, PackageLevelUpdateRequest request)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;

            _mapper.Map(request, entity);
            await _repo.UpdateAsync(entity);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;

            await _repo.DeleteAsync(entity);
            return true;
        }
    }
}
