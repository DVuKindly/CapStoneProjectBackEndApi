using AutoMapper;
using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Entities;

namespace MembershipService.API.Mappings
{
    public class BasicPackageProfile : Profile
    {
        public BasicPackageProfile()
        {
            CreateMap<CreateBasicPlanRequest, BasicPlan>();
            CreateMap<UpdateBasicPlanRequest, BasicPlan>();

            CreateMap<BasicPlan, BasicPlanResponseDto>()
                .ForMember(dest => dest.ServiceIds, opt => opt.Ignore())
                .ForMember(dest => dest.LocationName, opt => opt.MapFrom(src => src.Location.Name))
                .ForMember(dest => dest.PlanCategoryName, opt => opt.MapFrom(src => src.PlanCategory.Name));
        }
    }
}
