using CMS_HotelBooking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS_HotelBooking.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TestReminderController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IEmailService _emailService;

        public TestReminderController(
            IBookingService bookingService,
            IEmailService emailService)
        {
            _bookingService = bookingService;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<IActionResult> Send(int id = 4)
        {
            var booking = await _bookingService.GetByIdAsync(id);

            if (booking == null)
                return Content($"Booking {id} not found.");

            var result = await _emailService.SendBookingReminderAsync(
                booking,
                "24H"
            );

            return Content(
                $"Reminder test completed. BookingId: {id}, Email Status: {result}"
            );
        }
    }
}