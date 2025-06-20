using AutoMapper;
using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Entities;

namespace MembershipService.API.Mappings
{
    public class ComboPlanProfile : Profile
    {
        public ComboPlanProfile()
        {
            CreateMap<ComboPlanCreateRequest, ComboPlan>();
            CreateMap<ComboPlanUpdateRequest, ComboPlan>();

            CreateMap<ComboPlan, ComboPlanResponse>()
                .ForMember(dest => dest.NextUServiceIds, opt => opt.Ignore())
                .ForMember(dest => dest.PackageLevelName, opt => opt.MapFrom(src => src.PackageLevel.Name))
                .ForMember(dest => dest.LocationName, opt => opt.MapFrom(src => src.Location.Name))
                .ForMember(dest => dest.BasicPlanName, opt => opt.MapFrom(src => src.BasicPlan.Name));
        }
    }
}
