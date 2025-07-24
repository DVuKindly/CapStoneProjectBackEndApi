using AutoMapper;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Repositories.Interfaces;
using MembershipService.API.Services.Interfaces;

namespace MembershipService.API.Services.Implementations
{
    public class PlanCategoryService : IPlanCategoryService
    {
        private readonly IPlanCategoryRepository _repository;
        private readonly IMapper _mapper;

        public PlanCategoryService(IPlanCategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PlanCategoryDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<List<PlanCategoryDto>>(entities);
        }
    }
}