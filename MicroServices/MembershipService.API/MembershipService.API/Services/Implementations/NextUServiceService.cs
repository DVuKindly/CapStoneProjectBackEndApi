using AutoMapper;
using Azure.Core;
using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Entities;
using MembershipService.API.Enums;
using MembershipService.API.Repositories.Interfaces;
using MembershipService.API.Services.Interfaces;
using Sprache;
using static MembershipService.API.Dtos.Request.CreateNextUServiceRequest;

namespace MembershipService.API.Services.Implementations
{
    public class NextUServiceService : INextUServiceService
    {
        private readonly INextUServiceRepository _repositoryNextUSer;
        private readonly IEcosystemRepository _repositoryEco;
        private readonly IBasicPlanRepository _repositoryBasic;
        private readonly IMapper _mapper;

        public NextUServiceService(INextUServiceRepository repositoryNextUSer, IEcosystemRepository repositoryEco, IBasicPlanRepository repositoryBasic, IMapper mapper)
        {
            _repositoryNextUSer = repositoryNextUSer;
            _repositoryEco = repositoryEco;
            _repositoryBasic = repositoryBasic;
            _mapper = mapper;
        }

        public async Task<NextUServiceResponseDto> CreateAsync(CreateNextUServiceRequest request)
        {
            var entity = _mapper.Map<NextUService>(request);
            var result = await _repositoryNextUSer.AddAsync(entity);
            return _mapper.Map<NextUServiceResponseDto>(result);
        }


        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repositoryNextUSer.DeleteAsync(id);
        }

        public async Task<List<NextUServiceResponseDto>> GetAllAsync()
        {
            var result = await _repositoryNextUSer.GetAllAsync();
            return _mapper.Map<List<NextUServiceResponseDto>>(result);
        }

        public Task<List<NextUServiceResponseDto>> GetByBasicPlanIdAsync(Guid basicPlanId)
        {
            throw new NotImplementedException();
        }

        public async Task<NextUServiceResponseDto> GetByIdAsync(Guid id)
        {
            var result = await _repositoryNextUSer.GetByIdAsync(id);
            return _mapper.Map<NextUServiceResponseDto>(result);
        }

        public async Task<List<NextUServiceResponseDto>> GetByServiceTypeAsync(ServiceType type)
        {
            var result = await _repositoryNextUSer.GetByServiceTypeAsync(type);
            return _mapper.Map<List<NextUServiceResponseDto>>(result);
        }

        



        public async Task<NextUServiceResponseDto> UpdateAsync(Guid id, UpdateNextUServiceRequest request)
        {
            var entity = await _repositoryNextUSer.GetByIdAsync(id);
            if (entity == null) throw new Exception("NextUService not found");

            _mapper.Map(request, entity);
            var updated = await _repositoryNextUSer.UpdateAsync(entity);
            return _mapper.Map<NextUServiceResponseDto>(updated);
        }
    }
}

