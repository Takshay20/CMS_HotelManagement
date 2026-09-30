using CMS_HotelBooking.Models;
using CMS_HotelBooking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS_HotelBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BookingCalendarController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IRoomService _roomService;
        private readonly IRoomCategoryService _roomCategoryService;

        public BookingCalendarController(
            IBookingService bookingService,
            IRoomService roomService,
            IRoomCategoryService roomCategoryService)
        {
            _bookingService = bookingService;
            _roomService = roomService;
            _roomCategoryService = roomCategoryService;
        }

        // Open calendar page
        public IActionResult Calendar()
        {
            return View();
        }

        // Get categories
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _roomCategoryService.GetAllAsync();
            return Json(ResponseModel.SuccessResponse("Success", categories));
        }

        // Get month data
        [HttpGet]
        public async Task<IActionResult> GetMonthData(int year, int month, int categoryId = 0)
        {
            var monthStart = new DateTime(year, month, 1);
            var monthEnd = monthStart.AddMonths(1).AddDays(-1);

            var allRooms = await _roomService.GetAllAsync();
            var rooms = (categoryId > 0 ? allRooms.Where(r => r.RoomCategoryId == categoryId) : allRooms)
                        .OrderBy(r => r.RoomNumber)
                        .Select(r => new
                        {
                            r.RoomId,
                            r.RoomNumber,
                            r.Title,
                            r.RoomCategoryId
                        })
                        .ToList();
            var allBookings = await _bookingService.GetAllAsync();
            var bookings = allBookings
                .Where(b => b.Status == "Pending" || b.Status == "Approved")
                .Where(b => b.CheckInDate <= monthEnd && b.CheckOutDate >= monthStart)
                .Select(b => new
                {
                    b.BookingId,
                    b.RoomId,
                    b.FullName,
                    b.Status,
                    CheckInDate = b.CheckInDate.ToString("yyyy-MM-dd"),
                    CheckOutDate = b.CheckOutDate.ToString("yyyy-MM-dd"),
                    b.Guests
                })
                .ToList();
            return Json(ResponseModel.SuccessResponse("Success", new { rooms, bookings }));
        }
    }
}
