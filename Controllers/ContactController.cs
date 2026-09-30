using CMS_HotelBooking.Models;
using CMS_HotelBooking.Services.Interfaces;
using CMS_HotelBooking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS_HotelBooking.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactMessageService _contactMessageService;

        public ContactController(IContactMessageService contactMessageService)
        {
            _contactMessageService = contactMessageService;
        }

        // Open index page
        public IActionResult Index()
        {
            return View();
        }


        // Submit contact form
        [HttpPost]
        public async Task<IActionResult> Submit(ContactVM model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new
                {
                    success = false,
                    loginRequired = true,
                    message = "Please login to submit the form."
                });
            }

            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                    message = "Please fill in all required fields correctly."
                });
            }

            var message = new ContactMessage
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Subject = model.Subject,
                Message = model.Message
            };

            var result = await _contactMessageService.SubmitAsync(message);

            if (result > 0)
            {
                return Json(new
                {
                    success = true,
                    message = "Thank you for reaching out! We will get back to you shortly."
                });
            }

            return Json(new
            {
                success = false,
                message = "Something went wrong. Please try again."
            });
        }

        // Open track message page
        public IActionResult TrackMessage()
        {
            return View(new List<ContactMessage>());
        }

        // Submit track message form
        [HttpPost]
        public async Task<IActionResult> TrackMessage(string email)
        {
            var messages = string.IsNullOrWhiteSpace(email)
                ? new List<ContactMessage>()
                : await _contactMessageService.GetByEmailAsync(email.Trim());

            ViewBag.SearchedEmail = email;
            ViewBag.Searched = true;

            return View(messages);
        }
    }
}