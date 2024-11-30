using Final_Project_3amal.Data;
using Final_Project_3amal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Claims;

namespace Final_Project_3amal.Controllers
{
    public class UsersController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public UsersController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, IWebHostEnvironment hostingEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> Index(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            User u = user;

            var service = await _context.Services
               .Where(s => s.UserId == id)
            .ToListAsync();
            var serviceIds = service.Select(s => s.Id).ToList();

            var catygory = _context.Categories.ToList();

            var imageServices = _context.ImageServices
                .Where(img => serviceIds.Contains(img.ServiceId))
                .GroupBy(img => img.ServiceId)
                .Select(group => group.FirstOrDefault())
                .ToList();

            var model = (Users: u, Services: service, ImageServices: imageServices, Categories: catygory);

            return View(model);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, string Password, string ConfirmPassword, string Email, string FirstName, string LastName, string Phone, IFormFile Image)
        {
            if (id != int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                return NotFound();
            }

            // Retrieve the existing user from the database
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Update the fields
            if (FirstName != null)
            {
                user.FirstName = FirstName;

            }
            if (LastName != null)
            {
                user.LastName = LastName;

            }
            if (Phone != null)
            {
                user.Phone = Phone;

            }

            if (Image != null && Image.Length > 0)
            {
                var fileName = Path.GetFileName(Image.FileName);
                var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "Images", "Users", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Image.CopyToAsync(stream);
                }
                user.Imagebath = $"/Images/Users/{fileName}";
            }

            if (!string.IsNullOrEmpty(Email) && Email != user.Email)
            {
                var existingUser = _context.Users.FirstOrDefault(u => u.Email == Email);
                if (existingUser != null)
                {
                    ViewBag.Error = "Email already exists";
                    return View(user); // Return the same view with error
                }
                user.Email = Email;
            }

            if (!string.IsNullOrEmpty(Password))
            {
                if (Password != ConfirmPassword)
                {
                    ViewBag.Error = "Password does not match";
                    return View(user); // Return the same view with error
                }

                var passwordHasher = new PasswordHasher<User>();
                user.Password = passwordHasher.HashPassword(user, Password);
            }

            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                ViewBag.Error = "An error occurred while saving changes.";
                return View(user); // Return the same view with error
            }

            return RedirectToAction("Edit", "Users", new { id = user.Id });
        }

    }
}
