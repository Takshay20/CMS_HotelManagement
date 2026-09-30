using CMS_HotelBooking.Models;
using CMS_HotelBooking.Services.Interfaces;
using CMS_HotelBooking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMS_HotelBooking.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IRoomService _roomService;
        private readonly IPaymentService _paymentService;

        public BookingController(
            IBookingService bookingService,
            IRoomService roomService,
            IPaymentService paymentService)
        {
            _bookingService = bookingService;
            _roomService = roomService;
            _paymentService = paymentService;
        }

        // My bookings
        public async Task<IActionResult> MyBookings()
        {
            var userId =
                int.Parse(
                    User.FindFirstValue(ClaimTypes.NameIdentifier)!
                );

            var bookings =
                await _bookingService.GetByUserIdAsync(userId);

            var paymentStatuses =
                new Dictionary<int, string>();

            foreach (var booking in bookings)
            {
                var payment =
                    await _paymentService
                        .GetByBookingIdAsync(booking.BookingId);

                paymentStatuses[booking.BookingId] =
                    payment?.PaymentStatus ?? "NotPaid";
            }

            ViewBag.PaymentStatuses = paymentStatuses;

            return View(bookings);
        }

        // Create booking record
        public async Task<IActionResult> Create(int roomId)
        {
            var room = await _roomService.GetByIdAsync(roomId);
            if (room == null) return NotFound();
            return View(room);
        }

        // Check availability
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> CheckAvailability(int roomId, DateTime checkIn, DateTime checkOut)
        {
            var (available, message) = await _bookingService.CheckAvailabilityAsync(roomId, checkIn, checkOut);
            return Json(new { available, message });
        }

        // Submit create form
        [HttpPost]
        public async Task<IActionResult> Create(BookingVM model)
        {
            if (!ModelState.IsValid)
                return Json(ResponseModel.ErrorResponse("Please fill in all required fields correctly."));

            int? userId = null;
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null) userId = int.Parse(claim.Value);

            var booking = new Booking
            {
                UserId = userId,
                RoomId = model.RoomId,
                FullName = model.FullName,
                Email = model.Email,
                Phone = model.Phone,
                CheckInDate = model.CheckInDate,
                CheckOutDate = model.CheckOutDate,
                Guests = model.Guests,
                SpecialRequest = model.SpecialRequest
            };

            var (success, message) = await _bookingService.CreateAsync(booking);

            return success
                ? Json(ResponseModel.SuccessResponse(message))
                : Json(ResponseModel.ErrorResponse(message));
        }

        // Cancel booking
        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var (success, message, refundPercentage, refundAmount) = await _bookingService.CancelByGuestAsync(id, userId);

            return success
                ? Json(ResponseModel.SuccessResponse(message, new { refundPercentage, refundAmount }))
                : Json(ResponseModel.ErrorResponse(message));
        }

        // Respond room change
        [HttpPost]
        public async Task<IActionResult> RespondRoomChange(int id, bool accept)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var (success, message) = await _bookingService.RespondRoomChangeAsync(id, userId, accept);
            return success
                ? Json(ResponseModel.SuccessResponse(message))
                : Json(ResponseModel.ErrorResponse(message));
        }
    }
}
