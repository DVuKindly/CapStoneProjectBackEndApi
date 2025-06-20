using AutoMapper;
using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Entities;

namespace MembershipService.API.Mappings
{
    public class PackageLevelProfile : Profile
    {
        public PackageLevelProfile()
        {
            CreateMap<PackageLevel, PackageLevelResponse>();
            CreateMap<PackageLevelCreateRequest, PackageLevel>();
            CreateMap<PackageLevelUpdateRequest, PackageLevel>();
        }
    }
}
