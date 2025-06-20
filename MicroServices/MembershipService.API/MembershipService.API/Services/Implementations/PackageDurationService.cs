using AutoMapper;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Repositories.Interfaces;
using MembershipService.API.Services.Interfaces;

namespace MembershipService.API.Services.Implementations
{
    public class PackageDurationService : IPackageDurationService
    {
        private readonly IPackageDurationRepository _repository;
        private readonly IMapper _mapper;

        public PackageDurationService(IPackageDurationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PackageDurationResponse>> GetAllAsync()
        {
            var durations = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<PackageDurationResponse>>(durations);
        }

        public async Task<PackageDurationResponse?> GetByIdAsync(int id)
        {
            var duration = await _repository.GetByIdAsync(id);
            return duration == null ? null : _mapper.Map<PackageDurationResponse>(duration);
        }
    }
}
