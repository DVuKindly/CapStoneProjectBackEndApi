using AutoMapper;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Repositories.Interfaces;
using MembershipService.API.Services.Interfaces;

namespace MembershipService.API.Services.Implementations
{
    public class PlanTargetAudienceService : IPlanTargetAudienceService
    {
        private readonly IPlanTargetAudienceRepository _repository;
        private readonly IMapper _mapper;

        public PlanTargetAudienceService(IPlanTargetAudienceRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PlanTargetAudienceDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<List<PlanTargetAudienceDto>>(entities);
        }
    }
}