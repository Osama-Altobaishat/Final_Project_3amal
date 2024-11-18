using Final_Project_3amal.Data;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project_3amal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("search")]
        public IActionResult Search(string query)
        {
            if (string.IsNullOrEmpty(query))
                return Ok(new List<string>());

            var results = _context.Services
                .Where(x => x.Name.Contains(query)) // Adjust to your search logic
                .Select(x => new { x.Id, x.Name })  // Select necessary fields
                .ToList();

            return Ok(results);
        }
    }

}
