using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CampusRooms.Data;
using CampusRooms.Models;

namespace CampusRooms.Controllers
{
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Rooms
        public async Task<IActionResult> Index()
        {
            var rooms = await _context.Rooms
                .Include(r => r.Building)
                .OrderBy(r => r.Code)
                .ToListAsync();
            return View(rooms);
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var room = await _context.Rooms
                .Include(r => r.Building)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (room == null) return NotFound();

            return View(room);
        }

        // GET: Rooms/Create
        [Authorize(Policy = "CanManageRooms")]
        public IActionResult Create()
        {
            ViewData["BuildingId"] = new SelectList(_context.Buildings.OrderBy(b => b.Name), "Id", "Name");
            return View();
        }

        // POST: Rooms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CanManageRooms")]
        public async Task<IActionResult> Create([Bind("Code,Name,Seats,HasProjector,BuildingId")] Room room)
        {
            if (room.BuildingId == 0)
            {
                ModelState.AddModelError("BuildingId", "Pick a building");
            }

            if (ModelState.IsValid)
            {
                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BuildingId"] = new SelectList(_context.Buildings.OrderBy(b => b.Name), "Id", "Name", room.BuildingId);
            return View(room);
        }

        // GET: Rooms/Edit/5
        [Authorize(Policy = "CanManageRooms")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return NotFound();
            ViewData["BuildingId"] = new SelectList(_context.Buildings.OrderBy(b => b.Name), "Id", "Name", room.BuildingId);
            return View(room);
        }

        // POST: Rooms/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CanManageRooms")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,Name,Seats,HasProjector,BuildingId")] Room room)
        {
            if (id != room.Id) return NotFound();

            if (room.BuildingId == 0)
            {
                ModelState.AddModelError("BuildingId", "Pick a building");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.Id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BuildingId"] = new SelectList(_context.Buildings.OrderBy(b => b.Name), "Id", "Name", room.BuildingId);
            return View(room);
        }

        // GET: Rooms/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var room = await _context.Rooms
                .Include(r => r.Building)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (room == null) return NotFound();

            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: /rooms/seats/{min}  – Task 4: raw SQL
        [HttpGet("/rooms/seats/{min:int}")]
        public async Task<IActionResult> BigRooms(int min)
        {
            // Write the SELECT ourselves; EF Core still materialises the rows into Room objects.
            // The number is passed as a parameter (never concatenated into the SQL text).
            var rooms = await _context.Rooms
                .FromSqlRaw(
                    "SELECT Id, Code, Name, Seats, HasProjector, BuildingId FROM Rooms WHERE Seats >= {0} ORDER BY Seats DESC",
                    min)
                .Include(r => r.Building)
                .AsNoTracking()
                .ToListAsync();

            return View("Index", rooms);
        }

        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.Id == id);
        }
    }
}
