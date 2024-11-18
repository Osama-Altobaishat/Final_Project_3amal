using Final_Project_3amal.Data;
using Final_Project_3amal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Final_Project_3amal.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;


        public ProfileController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Correct way to access the current user's ID

            var categories = _context.Categories.ToList();

            var services = _context.Services
                .Where(s => s.UserId == int.Parse(userId))
                .Include(s => s.Category)
                .ToList();

            var serviceIds = services.Select(s => s.Id).ToList();
            var userIds = services.Select(s => s.UserId).ToList();

            var imageServices = _context.ImageServices
                .Where(img => serviceIds.Contains(img.ServiceId))
                .GroupBy(img => img.ServiceId)
                .Select(group => group.FirstOrDefault())
                .ToList();
            var users = _context.Users.Where(u => userIds.Contains(u.Id)).ToList();


            var model = ( Users: users, Services: services, Categories: categories, ImageServices: imageServices);

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateService(Service service, List<IFormFile> image, int CategoryId, float Price)
        {
            ModelState.Remove("User");
            ModelState.Remove("Category");

            if (!ModelState.IsValid || image == null || image.Count == 0 || Price <= 0)
            {
                return RedirectToAction("Index", "Profile");
            }

            service.CategoryId = CategoryId;
            service.Price = (decimal)Price;

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            service.UserId = int.Parse(userId);

            _context.Add(service);
            await _context.SaveChangesAsync();

            foreach (var file in image)
            {
                if (file.Length > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "Images", "Services", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    var imageService = new ImageService
                    {
                        ImagePath = $"/Images/Services/{fileName}",
                        ServiceId = service.Id
                    };
                    _context.Add(imageService);
                }

            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        
    }
}
