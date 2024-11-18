using Final_Project_3amal.Data;
using Final_Project_3amal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Final_Project_3amal.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public RegistrationController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, IWebHostEnvironment hostingEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Users()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Users(string FirstName, string LastName, string Email, string Password, string ConfirmPassword, int mobileNamber)
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
                Role = UserRole.User,
                Phone = mobileNamber.ToString(),
                Active = true,
                AboutUs = "No About Us",
                BirthDate = DateTime.Now,
                Imagebath = "No Image",
                Location = "No Location",

            };
            _context.Users.Add(u);
            _context.SaveChanges();

            return RedirectToAction("Index", "Login");
        }

        public IActionResult ServiceProvider()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ServiceProvider(User serviceProvider,string ConfirmPassword,string City, IFormFile Image)
        {
            ModelState.Remove("Imagebath");
            //ModelState.Remove("Image");

            if (ModelState.IsValid)
            {
                if (Image != null && Image.Length > 0)
                {
                    var fileName = Path.GetFileName(Image.FileName);
                    var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "Images", "Users", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(stream);
                    }
                    var existingUser = _context.Users.FirstOrDefault(u => u.Email == serviceProvider.Email);
                    if (existingUser != null)
                    {
                        ViewBag.Error = "Email already exists";
                        return View();
                    }
                    serviceProvider.Role = UserRole.Service_Provider;

                    serviceProvider.Location += @$"/{City}";
                    serviceProvider.Active = true;

                    var passwordHasher = new PasswordHasher<User>();

                    var SP = new User
                    {
                        FirstName = serviceProvider.FirstName,
                        LastName = serviceProvider.LastName,
                        Email = serviceProvider.Email,
                        Password = passwordHasher.HashPassword(null, serviceProvider.Password),
                        Role = serviceProvider.Role,
                        Phone = serviceProvider.Phone,
                        BirthDate = serviceProvider.BirthDate,
                        Imagebath = $"/Images/Users/{fileName}",
                        Location = serviceProvider.Location,
                        AboutUs = serviceProvider.AboutUs,
                        Active = serviceProvider.Active

                    };
                    if (serviceProvider.Password == ConfirmPassword)
                    {

                        _context.Users.Add(SP);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index", "Login");
                    }
                    else
                    {
                        ViewBag.Error = "Password does not match";
                    }
                }

            }
            

            return View();
        }
    }
}
