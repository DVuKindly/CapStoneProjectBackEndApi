using MembershipService.API.Dtos.Response;
using System.ComponentModel.DataAnnotations.Schema;
using MembershipService.API.Entities.Common;
using MembershipService.API.Enums;

namespace MembershipService.API.Entities
{
    public class RoomInstance : AuditableEntity
    {
        public Guid Id { get; set; }

        public Guid AccommodationOptionId { get; set; }
        public AccommodationOption AccommodationOption { get; set; }

        public string RoomCode { get; set; } = null!;
        public string RoomName { get; set; }
        public RoomStatus Status { get; set; } = RoomStatus.Available;

        public string? DescriptionDetails { get; set; }

        // Diện tích cụ thể
        public float? AreaInSquareMeters { get; set; }

        // Kích cỡ
        public int? RoomSizeOptionId { get; set; }
        public RoomSizeOption? RoomSizeOption { get; set; }

        // Hướng nhìn
        public int? RoomViewOptionId { get; set; }
        public RoomViewOption? RoomViewOption { get; set; }

        // Tầng mô tả (thấp, cao, sân thượng)
        public int? RoomFloorOptionId { get; set; }
        public RoomFloorOption? RoomFloorOption { get; set; }

        // Loại giường
        public int? BedTypeOptionId { get; set; }
        public BedTypeOption? BedTypeOption { get; set; }

        public int? NumberOfBeds { get; set; }

        public decimal? AddOnFee { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        [NotMapped]
        public List<MediaResponseDto> Medias { get; set; } = new();

    }

}