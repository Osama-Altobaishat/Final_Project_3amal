using Final_Project_3amal.Data;
using Final_Project_3amal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Final_Project_3amal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Catygory(string? Search)
        {

            var categories = _context.Categories.ToList();
            if (!string.IsNullOrEmpty(Search))
            {
                categories = categories.Where(u => u.Name.Contains(Search) ||
                                                   (int.TryParse(Search, out int searchId) && u.Id == searchId)).ToList();
            }

            return View(categories);
        }
        [HttpPost]
        public IActionResult CreateCategory(Category categorys)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(categorys);
                _context.SaveChanges();

                return RedirectToAction("Catygory", "Dashboard");
            }
            var categories = _context.Categories.ToList();
            return View("Catygory", categories);

        }
        [HttpPost]
        public IActionResult EditCategory(Category categorys)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Update(categorys);
                _context.SaveChanges();
                return RedirectToAction("Catygory", "Dashboard");
            }
            var categories = _context.Categories.ToList();
            return View("~/Views/Dashboard/Index.cshtml", categories);
        }
        [HttpPost]
        public IActionResult DeleteCategory(int id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }

            return RedirectToAction("Catygory", "Dashboard");
        }
        public IActionResult Services(string? Search)
        {
            var services = _context.Services.ToList();
            if (!string.IsNullOrEmpty(Search))
            {
                services = services.Where(u => u.Name.Contains(Search) ||
                                                   (int.TryParse(Search, out int searchId) && u.Id == searchId))
                    .ToList();
            }
            var categories = _context.Categories.ToList();
            var users = _context.Users.ToList();
            var model = (Services: services, Categories: categories, Users: users);


            return View(model);
        }


        public IActionResult Users(string? Search)
        {
            var users = _context.Users.Where(u => u.Role == UserRole.User).ToList();
            if (!string.IsNullOrEmpty(Search))
            {
                users = users.Where(u => u.FirstName.Contains(Search) ||
                                          u.LastName.Contains(Search) ||
                                          (int.TryParse(Search, out int searchId) && u.Id == searchId))
                              .ToList();
            }
            return View(users);
        }
        public IActionResult Providers(string? Search)
        {
            var users = _context.Users.Where(u => u.Role == UserRole.Service_Provider).ToList();
            if (!string.IsNullOrEmpty(Search))
            {
                users = users.Where(u => u.FirstName.Contains(Search) ||
                                          u.LastName.Contains(Search) ||
                                          (int.TryParse(Search, out int searchId) && u.Id == searchId))
                              .ToList();
            }
            return View(users);
        }
        public IActionResult Admins(string? Search)
        {
            var users = _context.Users.Where(u => u.Role == UserRole.Admin).ToList();
            if (!string.IsNullOrEmpty(Search))
            {
                users = users.Where(u => u.FirstName.Contains(Search) ||
                                          u.LastName.Contains(Search) ||
                                          (int.TryParse(Search, out int searchId) && u.Id == searchId))
                              .ToList();
            }
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActiveStatus(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Toggle the Active status
            user.Active = !user.Active;

            // Save the changes
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Json(new { success = true, activeStatus = user.Active }); // Return the updated status
        }
        [HttpGet]
        public IActionResult CreateAdmin()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateAdmin(string FirstName, string LastName, string Email, string Password, string ConfirmPassword, int mobileNamber)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Users));
            }
            if (Password != ConfirmPassword)
            {
                ViewBag.Error = "Password does not match";

                return RedirectToAction(nameof(Users));
            }
            var passwordHasher = new PasswordHasher<User>();

            var u = new User
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Password = passwordHasher.HashPassword(null, Password),
                Role = UserRole.Admin,
                Phone = mobileNamber.ToString(),
                Active = true,
                AboutUs = "No About Us",
                BirthDate = DateTime.Now,
                Imagebath = "No Image",
                Location = "No Location",

            };
            _context.Users.Add(u);
            _context.SaveChanges();

            return RedirectToAction("CreateAdmin", "Dashboard");
        }

    }
}
