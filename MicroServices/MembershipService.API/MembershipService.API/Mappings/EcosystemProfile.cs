using AutoMapper;
using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Entities;

namespace MembershipService.API.Mappings
{
    public class EcosystemProfile : Profile
    {
        public EcosystemProfile()
        {
            CreateMap<Ecosystem, EcosystemResponseDto>();
            CreateMap<EcosystemRequestDto, Ecosystem>();
        }
    }
}
