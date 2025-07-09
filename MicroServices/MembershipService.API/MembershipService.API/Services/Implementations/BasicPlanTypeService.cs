using AutoMapper;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Repositories.Interfaces;
using MembershipService.API.Services.Interfaces;

namespace MembershipService.API.Services.Implementations
{
    public class BasicPlanTypeService : IBasicPlanTypeService
    {
        private readonly IBasicPlanTypeRepository _repository;
        private readonly IMapper _mapper;

        public BasicPlanTypeService(IBasicPlanTypeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<BasicPlanTypeResponseDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<List<BasicPlanTypeResponseDto>>(entities);
        }

        public async Task<BasicPlanTypeResponseDto?> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<BasicPlanTypeResponseDto>(entity);
        }
    }
}
