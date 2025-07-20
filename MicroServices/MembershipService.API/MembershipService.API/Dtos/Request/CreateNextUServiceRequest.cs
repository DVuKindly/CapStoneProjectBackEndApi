using MembershipService.API.Enums;

namespace MembershipService.API.Dtos.Request
{
    public class CreateNextUServiceRequest
    {
        public string Name { get; set; } = null!;
        public ServiceType ServiceType { get; set; }
        public Guid EcosystemId { get; set; }
        public Guid? PropertyId { get; set; }
    }
}
