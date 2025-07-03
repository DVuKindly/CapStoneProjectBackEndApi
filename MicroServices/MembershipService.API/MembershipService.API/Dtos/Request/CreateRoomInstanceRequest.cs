using MembershipService.API.Enums;

namespace MembershipService.API.Dtos.Request
{
    public class CreateRoomInstanceRequest
    {
        public Guid AccommodationOptionId { get; set; }
        public string RoomCode { get; set; }
        public string RoomName { get; set; }
        public string? DescriptionDetails { get; set; }
        public Floor Floor { get; set; }
    }
    public class UpdateRoomInstanceRequest : CreateRoomInstanceRequest
    {
        public RoomStatus Status { get; set; }
    }
}
