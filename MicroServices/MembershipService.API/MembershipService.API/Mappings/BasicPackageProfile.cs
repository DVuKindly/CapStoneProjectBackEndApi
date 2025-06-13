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
            CreateMap<BasicPackageCreateRequest, BasicPackage>();
            CreateMap<BasicPackageUpdateRequest, BasicPackage>();
            CreateMap<BasicPackage, BasicPackageResponse>()
                .ForMember(dest => dest.NextUServiceIds,
                    opt => opt.Ignore());
        }
    }
}
