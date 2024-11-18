using Final_Project_3amal.Data;
using Final_Project_3amal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Project_3amal.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) {
                return NotFound();
            }
            User u = user;

            var  service = await  _context.Services
               .Where(s => s.UserId >= id) // Filter by role
            .ToListAsync();
            var serviceIds = service.Select(s => s.Id).ToList();

            var catygory = _context.Categories.ToList();

            var imageServices = _context.ImageServices
                .Where(img => serviceIds.Contains(img.ServiceId))
                .GroupBy(img => img.ServiceId)
                .Select(group => group.FirstOrDefault())
                .ToList();
            
            var model = (Users: u, Services: service, ImageServices: imageServices ,Categories: catygory);

            return View(model);
        }
    }
}
