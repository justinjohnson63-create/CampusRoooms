namespace CampusRooms.Models
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Seats { get; set; }
        public bool HasProjector { get; set; }
        public int BuildingId { get; set; }
        public string BuildingName { get; set; } = string.Empty;
    }
}