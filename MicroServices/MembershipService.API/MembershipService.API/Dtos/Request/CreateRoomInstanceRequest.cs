using MembershipService.API.Enums;

namespace MembershipService.API.Dtos.Request
{
    public class CreateRoomInstanceRequest
    {
        public Guid AccommodationOptionId { get; set; }

        public string RoomCode { get; set; } = null!;
        public string RoomName { get; set; } = null!;
        public string? DescriptionDetails { get; set; }

        public float? AreaInSquareMeters { get; set; }

        public int? RoomSizeOptionId { get; set; }

        public int? RoomViewOptionId { get; set; }

        public int? RoomFloorOptionId { get; set; }

        public int? BedTypeOptionId { get; set; }

        public int? NumberOfBeds { get; set; }

        public decimal? AddOnFee { get; set; }



    }
    public class UpdateRoomInstanceRequest : CreateRoomInstanceRequest
    {
        public RoomStatus Status { get; set; }
    }
}
