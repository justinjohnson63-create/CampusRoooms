using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampusRooms.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UsersController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.OrderBy(u => u.Email).ToListAsync();
            var model = new List<UserViewModel>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                model.Add(new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email ?? "",
                    IsStaff = roles.Contains("Staff") || roles.Contains("Admin")
                });
            }
            return View(model);
        }

        // POST: Users/MakeStaff/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MakeStaff(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            if (!await _userManager.IsInRoleAsync(user, "Staff"))
            {
                await _userManager.AddToRoleAsync(user, "Staff");
                TempData["Message"] = $"{user.Email} is now Staff.";
            }
            return RedirectToAction(nameof(Index));
        }
    }

    public class UserViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsStaff { get; set; }
    }
}
