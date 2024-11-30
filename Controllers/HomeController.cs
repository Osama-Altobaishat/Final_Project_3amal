using Final_Project_3amal.Data;
using Final_Project_3amal.Migrations;
using Final_Project_3amal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace Final_Project_3amal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var catygory = _context.Categories.ToList();
            var user = await _context.Users
                .Where(s => s.Role == UserRole.Service_Provider) // Filter by role
                .OrderBy(x => Guid.NewGuid()) // Randomize order
                .Take(5) // Select only 5 users
                .ToListAsync();

            var service = await _context.Services
               .Where(s => s.RatingAvg >= 0) // Filter by role
               .OrderBy(x => Guid.NewGuid()) // Randomize order
               .Take(4) // Select only 5 users
            .ToListAsync();


            var serviceIds = service.Select(s => s.Id).ToList();

            var imageServices = _context.ImageServices
                .Where(img => serviceIds.Contains(img.ServiceId))
                .GroupBy(img => img.ServiceId)
                .Select(group => group.FirstOrDefault())
                .ToList();

            var model = (services: service, users: user, catygories: catygory, imageService: imageServices);

            return View(model);
        }

        public IActionResult About_us()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
