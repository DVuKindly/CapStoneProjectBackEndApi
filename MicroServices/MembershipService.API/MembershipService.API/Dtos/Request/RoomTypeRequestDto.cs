namespace MembershipService.API.Dtos.Request
{
    public class CreateRoomTypeRequest
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }

    public class UpdateRoomTypeRequest
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
