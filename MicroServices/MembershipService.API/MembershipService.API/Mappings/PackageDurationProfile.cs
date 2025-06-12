using AutoMapper;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Entities;

namespace MembershipService.API.Mappings
{
    public class PackageDurationProfile : Profile
    {
        public PackageDurationProfile()
        {
            CreateMap<PackageDuration, PackageDurationResponse>()
                .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.Unit.ToString()));
        }
    }
}
