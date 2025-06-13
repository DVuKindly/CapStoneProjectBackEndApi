namespace MembershipService.API.Dtos.Request
{
    public class NextUServiceRequestDto
    {
        public class CreateNextUServiceRequest
        {
            public string Name { get; set; }
            public string UnitType { get; set; }
            public Guid EcosystemId { get; set; }
        }

        public class UpdateNextUServiceRequest
        {
            public string Name { get; set; }
            public string UnitType { get; set; }
            public Guid EcosystemId { get; set; }
        }
    }
}
