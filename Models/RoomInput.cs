using System.ComponentModel.DataAnnotations;

namespace CampusRooms.Models
{
    public class RoomInput
    {
        [Required, StringLength(20)]
        public string Code { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(1, 300)]
        public int Seats { get; set; }

        public bool HasProjector { get; set; }

        [Required]
        public int BuildingId { get; set; }
    }
}