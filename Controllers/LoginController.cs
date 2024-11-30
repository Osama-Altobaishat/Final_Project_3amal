using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Final_Project_3amal.Models;
using Microsoft.EntityFrameworkCore;
using Final_Project_3amal.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Final_Project_3amal.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment environment;
        public LoginController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this._context = context;
            this.environment = environment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Dashboard(User user)
        {
            // Find user by email
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == user.Email);

            // Verify if user exists and password is correct
            if (existingUser != null && VerifyPassword(user.Password, existingUser.Password))
            {
                // Create claims for the user
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, existingUser.Id.ToString()),
                    new Claim(ClaimTypes.Email, existingUser.Email),
                    new Claim(ClaimTypes.MobilePhone, existingUser.Phone),
                    new Claim(ClaimTypes.Name, $"{existingUser.FirstName} {existingUser.LastName}"),
                    new Claim(ClaimTypes.Role, existingUser.Role.ToString()),
                };

                // Create claims identity
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Sign in the user with the created claims principal
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                // Get the user's services
                var services = await _context.Services
                    .Where(s => s.UserId == existingUser.Id)
                    .Include(s => s.Category)  // Optionally include related data like Category
                    .ToListAsync();

                // Pass services to the view
                ViewBag.Services = services;

                // Redirect to home page or desired location after successful login
                return RedirectToAction("Catygory", "Dashboard");
            }
            ModelState.AddModelError("", "Invalid username or password.");
            ViewBag.Error = "Invalid email or password.";
            return View(user);
        }
            [HttpPost]
        public async Task<IActionResult> Index(User user)
        {
            // Find user by email
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == user.Email);

            // Verify if user exists and password is correct
            if (existingUser != null && VerifyPassword(user.Password, existingUser.Password))
            {
                // Create claims for the user
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, existingUser.Id.ToString()),
                    new Claim(ClaimTypes.Email, existingUser.Email),
                    new Claim(ClaimTypes.MobilePhone, existingUser.Phone),
                    new Claim("About", existingUser.AboutUs),
                    new Claim("Location", existingUser.Location),
                    new Claim(ClaimTypes.DateOfBirth, existingUser.BirthDate.ToString()),
                    new Claim(ClaimTypes.Name, $"{existingUser.FirstName} {existingUser.LastName}"),
                    new Claim(ClaimTypes.Role, existingUser.Role.ToString()),
                    new Claim("ProfileImage", existingUser.Imagebath)
                };

                // Create claims identity
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Sign in the user with the created claims principal
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                // Get the user's services
                var services = await _context.Services
                    .Where(s => s.UserId == existingUser.Id)
                    .Include(s => s.Category)  // Optionally include related data like Category
                    .ToListAsync();

                // Pass services to the view
                ViewBag.Services = services;

                // Redirect to home page or desired location after successful login
                return RedirectToAction("Index", "Home");
            }

            // If login fails, show an error message
            ModelState.AddModelError("", "Invalid username or password.");
            ViewBag.Error = "Invalid username or password.";

            return View(user);
        }

        private bool VerifyPassword(string password, string passwordHash)
        {
            var passwordHasher = new PasswordHasher<User>();
            return passwordHasher.VerifyHashedPassword(null, passwordHash, password) == PasswordVerificationResult.Success;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogoutDashboard()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Dashboard", "Login");
        }
    }
}
