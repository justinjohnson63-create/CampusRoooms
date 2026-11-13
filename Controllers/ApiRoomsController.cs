using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CampusRooms.Data;
using CampusRooms.Models;

namespace CampusRooms.Controllers
{
    [Route("api/rooms")]
    [ApiController]
    public class ApiRoomsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ApiRoomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/rooms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomDto>>> GetRooms()
        {
            var rooms = await _context.Rooms
                .Include(r => r.Building)
                .OrderBy(r => r.Code)
                .Select(r => new RoomDto
                {
                    Id = r.Id,
                    Code = r.Code,
                    Name = r.Name,
                    Seats = r.Seats,
                    HasProjector = r.HasProjector,
                    BuildingId = r.BuildingId,
                    BuildingName = r.Building != null ? r.Building.Name : ""
                })
                .ToListAsync();
            return Ok(rooms);
        }

        // GET: api/rooms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomDto>> GetRoom(int id)
        {
            var room = await _context.Rooms
                .Include(r => r.Building)
                .Where(r => r.Id == id)
                .Select(r => new RoomDto
                {
                    Id = r.Id,
                    Code = r.Code,
                    Name = r.Name,
                    Seats = r.Seats,
                    HasProjector = r.HasProjector,
                    BuildingId = r.BuildingId,
                    BuildingName = r.Building != null ? r.Building.Name : ""
                })
                .FirstOrDefaultAsync();

            if (room == null) return NotFound();
            return Ok(room);
        }

        // POST: api/rooms
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<RoomDto>> PostRoom(RoomInput input)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Check unique code
            if (await _context.Rooms.AnyAsync(r => r.Code == input.Code))
            {
                return Conflict(new { message = "A room with this code already exists." });
            }

            // Check building exists
            if (!await _context.Buildings.AnyAsync(b => b.Id == input.BuildingId))
            {
                ModelState.AddModelError("BuildingId", "Building does not exist.");
                return BadRequest(ModelState);
            }

            var room = new Room
            {
                Code = input.Code,
                Name = input.Name,
                Seats = input.Seats,
                HasProjector = input.HasProjector,
                BuildingId = input.BuildingId
            };

            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            // Reload with building
            await _context.Entry(room).Reference(r => r.Building).LoadAsync();

            var dto = new RoomDto
            {
                Id = room.Id,
                Code = room.Code,
                Name = room.Name,
                Seats = room.Seats,
                HasProjector = room.HasProjector,
                BuildingId = room.BuildingId,
                BuildingName = room.Building?.Name ?? ""
            };

            return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, dto);
        }

        // PUT: api/rooms/5
        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> PutRoom(int id, RoomInput input)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return NotFound();

            // Check unique code (ignore self)
            if (await _context.Rooms.AnyAsync(r => r.Code == input.Code && r.Id != id))
            {
                return Conflict(new { message = "A room with this code already exists." });
            }

            if (!await _context.Buildings.AnyAsync(b => b.Id == input.BuildingId))
            {
                ModelState.AddModelError("BuildingId", "Building does not exist.");
                return BadRequest(ModelState);
            }

            room.Code = input.Code;
            room.Name = input.Name;
            room.Seats = input.Seats;
            room.HasProjector = input.HasProjector;
            room.BuildingId = input.BuildingId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Rooms.AnyAsync(r => r.Id == id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        // DELETE: api/rooms/5
        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return NotFound();

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
