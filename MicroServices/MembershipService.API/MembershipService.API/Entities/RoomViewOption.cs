namespace MembershipService.API.Entities
{
    public class RoomViewOption
    {
        public int Id { get; set; } // 1=Vườn, 2=Biển,...
        public string Name { get; set; } = null!;
        public ICollection<RoomInstance> Rooms { get; set; } = new List<RoomInstance>();

    }
}
