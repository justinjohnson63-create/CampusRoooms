using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusRooms.Models
{
    public class Room
    {
        public int Id { get; set; }

        [Required, StringLength(20)]
        public string Code { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(1, 300)]
        public int Seats { get; set; }

        public bool HasProjector { get; set; }

        [Required]
        public int BuildingId { get; set; }

        [ForeignKey(nameof(BuildingId))]
        public Building? Building { get; set; }
    }
}