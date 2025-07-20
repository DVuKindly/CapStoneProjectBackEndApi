using MembershipService.API.Enums;

namespace MembershipService.API.Dtos.Response
{
    public class NextUServiceResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ServiceType ServiceType { get; set; }
        public Guid EcosystemId { get; set; }
        public string EcosystemName { get; set; }
        public Guid PropertyId { get; set; }
        public string PropertyName { get; set; }

        public List<Guid> MediaGalleryId { get; set; }
    }
}
