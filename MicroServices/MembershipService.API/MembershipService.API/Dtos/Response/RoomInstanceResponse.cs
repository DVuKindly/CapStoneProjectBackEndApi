namespace MembershipService.API.Dtos.Response
{
    public class RoomInstanceResponse
    {
        public Guid Id { get; set; }
        public Guid AccommodationOptionId { get; set; }

        public string RoomCode { get; set; }
        public string RoomName { get; set; }
        public string? DescriptionDetails { get; set; }

        public string Status { get; set; }

        public float? AreaInSquareMeters { get; set; }
        public string? RoomSizeName { get; set; }
        public string? RoomViewName { get; set; }
        public string? RoomFloorName { get; set; }
        public string? BedTypeName { get; set; }
        public int? NumberOfBeds { get; set; }
        public string RoomTypeName { get; set; }
        public decimal? AddOnFee { get; set; }
        public Guid? PropertyId { get; set; }
        public string PropertyName { get; set; }
        public Guid? LocationId { get; set; }
        public string LocationName { get; set; }
        public Guid? CityId { get; set; }
        public string CityName { get; set; }
    }

}
