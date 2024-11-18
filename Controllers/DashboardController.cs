using Final_Project_3amal.Data;
using Final_Project_3amal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Final_Project_3amal.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(string Search)
        {

            var categories = _context.Categories.ToList();
            if (!string.IsNullOrEmpty(Search))
            {
                categories = categories.Where(u => u.Name.Contains(Search) ||
                                                   (int.TryParse(Search, out int searchId) && u.Id == searchId))
                                       .ToList();
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

                return RedirectToAction("Index", "Dashboard");
            }
            var categories = _context.Categories.ToList();
            return View("~/Views/Dashboard/Index.cshtml", categories);

        }
        [HttpPost]
        public IActionResult EditCategory(Category categorys)
        {
            if (ModelState.IsValid) {
                _context.Categories.Update(categorys);
                _context.SaveChanges();
                return RedirectToAction("Index", "Dashboard");
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

            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult Users()
        {
            var users = _context.Users.ToList();
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


    }
}
