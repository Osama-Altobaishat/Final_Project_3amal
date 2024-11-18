using Final_Project_3amal.Data;
using Final_Project_3amal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace Final_Project_3amal.Controllers
{

    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int id, string name, int pageNumber = 1, int pageSize = 9)
        {

            // Get the total count of services for the category to calculate total pages
            var totalServices = await _context.Services
                .Where(s => s.CategoryId == id)
                .CountAsync();

            // Apply pagination to the filtered services
            var services = await _context.Services
                .Where(s => s.CategoryId == id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Calculate total pages
            var totalPages = (int)Math.Ceiling((double)totalServices / pageSize);

            // View model to pass paginated data and pagination info
            var viewModel = new ServicesViewModel
            {
                Services = services,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages
            };
            
            var serviceIds = viewModel.Services.Select(s => s.Id).ToList();
            var userid = viewModel.Services.Select(s => s.UserId).ToList();

            // Fetch images for services
            var imageServices = _context.ImageServices
                .Where(img => serviceIds.Contains(img.ServiceId))
                .GroupBy(img => img.ServiceId)
                .Select(group => group.FirstOrDefault())
                .ToList();
            var users = _context.Users.Where(u => userid.Contains(u.Id)).ToList();

            // Fetch categories
            var category = _context.Categories.ToList();

            var model = (user: users, Categories: category, viewModels: viewModel, imageService: imageServices);

            // Pass data to the view
            ViewBag.CategoryId = id;
            ViewBag.Name = name;

            return View(model);
        }

    }
}
