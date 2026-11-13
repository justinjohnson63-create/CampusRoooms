using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CampusRooms.Data;

namespace CampusRooms.Controllers
{
    public class BuildingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BuildingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Buildings/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var building = await _context.Buildings
                .Include(b => b.Rooms)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (building == null) return NotFound();

            return View(building);
        }
    }
}
