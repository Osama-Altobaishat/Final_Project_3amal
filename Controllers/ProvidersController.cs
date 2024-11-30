using Final_Project_3amal.Data;
using Final_Project_3amal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Project_3amal.Controllers
{
    public class ProvidersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProvidersController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(string? Search, int? id, int pageNumber = 1, int pageSize = 6)
        {
            var users = _context.Users.Where(u => u.Role == UserRole.Service_Provider).ToList();
            if (!string.IsNullOrEmpty(Search))
            {
                users = users.Where(u => u.FirstName.Contains(Search) ||
                                          u.LastName.Contains(Search) )
                              .ToList();
            }
            var totalUsers = users.Count;
            users = users.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Calculate total pages
            var totalPages = (int)Math.Ceiling((double)totalUsers / pageSize);

            var model = (Users: users, TotalPages: totalPages, PageNumber: pageNumber, PageSize: pageSize);
            return View(model);
        }
    }
}
