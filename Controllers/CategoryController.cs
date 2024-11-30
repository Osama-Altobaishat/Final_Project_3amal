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
        public async Task<IActionResult> Index(int? id, string? name, int pageNumber = 1, int pageSize = 6, string? search = null, decimal? minPrice = null, decimal? maxPrice = null, decimal? Rating = null)
        {
            // Apply pagination to the filtered services
            var services = await _context.Services.ToListAsync();
                
            if (search != null && search != "")
            {
                services = services.Where(s => s.Name.Contains(search)).ToList();
                ViewBag.search = search;
            }
            if (minPrice != null && maxPrice != null)
            {
                services = services.Where(s => s.Price >= minPrice && s.Price <= maxPrice).ToList();
                ViewBag.minPrice = minPrice;
                ViewBag.maxPrice = maxPrice;
            }
            if (Rating != null && Rating != 0)
            {
                services = services.Where(s => s.RatingAvg >= Rating).ToList();
                ViewBag.Rating = Rating;
            }
            if (id != null && id != 0)
            {
                services = services.Where(s => s.CategoryId == id).ToList();
                // Pass data to the view
                ViewBag.CategoryId = id;
                ViewBag.Name = name;
            }
            if(services == null)
            {
                return NotFound();
            }
            var totalServices = services.Count();

            services =  services.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

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

            return View(model);
        }

    }
}
