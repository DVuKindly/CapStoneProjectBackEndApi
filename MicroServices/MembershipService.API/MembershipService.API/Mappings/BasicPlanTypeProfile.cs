using AutoMapper;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Entities;

namespace MembershipService.API.Mappings
{
    public class BasicPlanTypeProfile : Profile
    {
        public BasicPlanTypeProfile()
        {
            CreateMap<BasicPlanType, BasicPlanTypeResponseDto>();
        }
    }
}
