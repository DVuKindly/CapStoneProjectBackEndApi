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
            CreateMap<BasicPlanCreateRequest, BasicPlan>();
            CreateMap<BasicPackageUpdateRequest, BasicPlan>();

            CreateMap<BasicPlan, BasicPlanResponse>()
                .ForMember(dest => dest.NextUServiceIds, opt => opt.Ignore())
                .ForMember(dest => dest.PackageDurationName, opt => opt.MapFrom(src => src.PackageDuration.Description))
                .ForMember(dest => dest.LocationName, opt => opt.MapFrom(src => src.Location.Name));
        }
    }
}
