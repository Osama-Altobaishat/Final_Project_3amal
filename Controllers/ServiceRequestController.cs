﻿using Final_Project_3amal.Data;
using Final_Project_3amal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;

namespace Final_Project_3amal.Controllers
{
    [Authorize(Roles = "User,Service_Provider")]
    public class ServiceRequestController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServiceRequestController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult ServiceRequestUsers(int id, string name, string category, int categoryid, string description, decimal price, decimal ratingAvg, int userid)
        {
            Service service = new Service() { Id = id, Name = name,
                CategoryId = categoryid, Description = description, 
                Price = price, RatingAvg = ratingAvg, UserId = userid };

            var user = _context.Users.FirstOrDefault(u => u.Id == userid);

            var model = (Service: service, User: user, Category: category);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ServiceRequest(int serviceId, string locationNames, float lat, float lng, string serviceName, string serviceDesc, DateTime serviceDate, float serviceNumber)
        {
            if (ModelState.IsValid)
            {
                var service = await _context.Services.FindAsync(serviceId);
                if (service == null) { 
                    return NotFound();
                }
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
                if (user == null) {
                    return NotFound();
                }
                ServiceRequest serviceRequest = new ServiceRequest()
                {
                    Title = serviceName,
                    ServiceId = serviceId,
                    Location = locationNames,
                    lat = (decimal)lat,
                    lng = (decimal)lng,
                    Service = service,
                    User = user,
                    Description = serviceDesc,
                    StartDate = serviceDate,
                    Price = (decimal) serviceNumber,
                    Status = Status.Pending,
                    SendDate = DateTime.Now
                };
                _context.ServiceRequests.Add(serviceRequest);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return View();
        }
        public async Task<IActionResult> ServiceRequestList()
        {

            var ServiceRequest = await _context.ServiceRequests
                .Where(s => s.Service != null && s.Service.UserId == int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)))
                .ToListAsync();

            var ServiceRequestsid = ServiceRequest.Select(s => s.ServiceId).ToList();
            var ServiceRequestusersid = ServiceRequest.Select(s => s.UserId).ToList();

            var services = await _context.Services
                .Where(s => ServiceRequestsid.Contains(s.Id))
                .ToListAsync();

            var users = await _context.Users
                .Where(s => ServiceRequestusersid.Contains(s.Id))
                .ToListAsync();

            var model = (ServiceRequests: ServiceRequest, Services: services, Users: users);
            return View(model);
        }
        public async Task<IActionResult> ServiceRequestListUser()
        {

            var ServiceRequest = await _context.ServiceRequests
                .Where(s => s.Service != null && s.UserId == int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)))
                .ToListAsync();

            var ServiceRequestsid = ServiceRequest.Select(s => s.ServiceId).ToList();
            var ServiceRequestusersid = ServiceRequest.Select(s => s.UserId).ToList();

            var services = await _context.Services
                .Where(s => ServiceRequestsid.Contains(s.Id))
                .ToListAsync();

            var users = await _context.Users
                .Where(s => ServiceRequestusersid.Contains(s.Id))
                .ToListAsync();

            var model = (ServiceRequests: ServiceRequest, Services: services, Users: users);
            return View(model);
        }
        public async Task<IActionResult> ServiceRequestShow(int id)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            var serviceRequest = await _context.ServiceRequests.FirstOrDefaultAsync(sr => sr.Id == id);
            if (serviceRequest == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FirstOrDefaultAsync(sr => sr.Id == serviceRequest.UserId);
            var service = await _context.Services.FirstOrDefaultAsync(sr => sr.Id == serviceRequest.ServiceId);
            var model = (ServiceRequest: serviceRequest, Service: service, User: user);
            return View(model);
        }
        public async Task<IActionResult> ServiceRequestShowUser(int id)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            var serviceRequest = await _context.ServiceRequests.FirstOrDefaultAsync(sr => sr.Id == id);
            if (serviceRequest == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FirstOrDefaultAsync(sr => sr.Id == serviceRequest.UserId);
            var service = await _context.Services.FirstOrDefaultAsync(sr => sr.Id == serviceRequest.ServiceId);
            var model = (ServiceRequest: serviceRequest, Service: service, User: user);
            return View(model);
        }
        [HttpPost]
        public IActionResult UpdateStatus(int requestId, string action)
        {
            // Retrieve the service request from the database
            var serviceRequest = _context.ServiceRequests.FirstOrDefault(r => r.Id == requestId);

            if (serviceRequest == null)
            {
                return Json(new { success = false, message = "Request not found." });
            }

            // Update the status based on the action
            switch (action.ToLower())
            {
                case "accept":
                    serviceRequest.Status = Status.Accepted;
                    break;
                case "reject":
                    serviceRequest.Status = Status.Rejected;
                    break;
                case "cancel":
                    serviceRequest.Status = Status.Cancelled;
                    break;
                case "complete":
                    serviceRequest.Status = Status.Completed;
                    serviceRequest.FinishDate = DateTime.Now;
                    break;
                default:
                    return Json(new { success = false, message = "Invalid action." });
            }

            // Save changes to the database
            _context.SaveChanges();

            return Json(new { success = true, message = "Status updated successfully." });
        }
        [HttpPost]
        public async Task<IActionResult> AddReview(int id,int serviceId, float rating, string comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var servicerequest = await _context.ServiceRequests.FirstOrDefaultAsync(sr => sr.Id == id);
            if (servicerequest == null)
            {
                return NotFound();
            }
            servicerequest.IsReviewed = true;
            var service = await _context.Services.FirstOrDefaultAsync(sr => sr.Id == serviceId);
            if (service == null)
            {
                return NotFound();
            }
            var reviews = await _context.Reviews.Where(sr => sr.ServiceId == serviceId).ToListAsync();
            service.RatingAvg = (service.RatingAvg + (decimal)rating) / (reviews.Count + 1);

            var review = new Review()
            {
                ServiceId = serviceId,
                UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                RatingCommunicator = (decimal)rating,
                Comment = comment
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return Ok();

        }

    }
}
