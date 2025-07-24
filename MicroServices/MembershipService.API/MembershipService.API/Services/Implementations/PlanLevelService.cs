using AutoMapper;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Repositories.Interfaces;
using MembershipService.API.Services.Interfaces;

namespace MembershipService.API.Services.Implementations
{
    public class PlanLevelService : IPlanLevelService
    {
        private readonly IPlanLevelRepository _repository;
        private readonly IMapper _mapper;

        public PlanLevelService(IPlanLevelRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PlanLevelDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<List<PlanLevelDto>>(entities);
        }
    }
}