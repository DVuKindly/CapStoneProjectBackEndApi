using AutoMapper;
using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Entities;

namespace MembershipService.API.Mappings
{
    public class EntitlementRuleProfile : Profile
    {
        public EntitlementRuleProfile()
        {
            CreateMap<EntitlementRule, EntitlementRuleDto>();
            CreateMap<CreateEntitlementRuleDto, EntitlementRule>();
            CreateMap<UpdateEntitlementRuleDto, EntitlementRule>();
        }
    }
}
