﻿using Final_Project_3amal.Data;
using Final_Project_3amal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Final_Project_3amal.Controllers
{
    public class ServesInfoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServesInfoController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int id, string name, string category, int categoryid, string description, decimal price, decimal ratingAvg,int userid)
        {
            if(id == 0 || name == null || category == null || categoryid == 0 || description == null)
            {
                return NotFound();
            }
            ViewBag.Id = id;
            ViewBag.Name = name;
            ViewBag.CategoryName = category;
            ViewBag.CategoryId = categoryid;
            ViewBag.Description = description;
            ViewBag.Price = price;
            ViewBag.RatingAvg = ratingAvg;
            ViewBag.userId = userid;

            var imageServices = _context.ImageServices
                .Where(img => img.ServiceId == id)  
                .ToList();

            var reviews = _context.Reviews
                .Where(re => re.ServiceId == id)  // Filter by single service ID
                .ToList();
            var service =  _context.Services
               .Where(s => s.RatingAvg >= 0 && s.CategoryId == categoryid) // Filter by role
               .OrderBy(x => Guid.NewGuid()) // Randomize order
               .Take(3) // Select only 5 users
                .ToList();
            var serviceIds = service.Select(s => s.Id).ToList();

            var imageServices2 = _context.ImageServices
                .Where(img => serviceIds.Contains(img.ServiceId))
                .ToList();

            var catygory = _context.Categories.ToList();

            var user = _context.Users.FirstOrDefault(u => u.Id == userid);
            var iduserreviews = reviews.Select(x => x.UserId).ToList();
            var reviewsUsers = _context.Users
                .Where(re => iduserreviews.Contains(re.Id))  // Use Contains to filter by a list
                .ToList();


            var model = (
                Users: user,
                ImageServices: imageServices,
                ImageServices2: imageServices2,
                Reviews: reviews, 
                Services: service, 
                Categories: catygory,
                ReviewsUsers: reviewsUsers
            );

            return View(model);

        }
    }
}
