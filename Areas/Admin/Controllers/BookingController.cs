using CMS_HotelBooking.Models;
using CMS_HotelBooking.Services.Implementations;
using CMS_HotelBooking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS_HotelBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BookingController : Controller
    {
        private readonly IBookingService _service;
        private readonly IPaymentService _paymentService;

        public BookingController(IBookingService service,IPaymentService paymentService)
        {
            _service = service;
            _paymentService = paymentService;
        }

        // Open booking page
        public IActionResult Booking()
        {
            return View();
        }

        // Get all booking records
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();

            return Json(
                ResponseModel.SuccessResponse(
                    "Success",
                    result
                )
            );
        }

        // Get booking record by id
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
            {
                return Json(
                    ResponseModel.ErrorResponse(
                        "Record not found."
                    )
                );
            }

            return Json(
                ResponseModel.SuccessResponse(
                    "Success",
                    result
                )
            );
        }

        // Update status
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int bookingId,string status)
        {
            var (result, emailNote) = await _service.UpdateStatusWithEmailAsync(
                    bookingId,
                    status
                );

            if (result > 0)
            {
                return Json(
                        ResponseModel.SuccessResponse(
                        $"Booking marked as {status}.{emailNote}"
                    )
                );
            }

            return Json(
                ResponseModel.ErrorResponse(
                    "Unable To Update Booking"
                )
            );
        }

        // Delete booking record
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);

            if (result > 0)
            {
                return Json(
                    ResponseModel.SuccessResponse(
                        "Record Deleted Successfully."
                    )
                );
            }

            return Json(
                ResponseModel.ErrorResponse(
                    "Unable To Delete Record"
                )
            );
        }

        // Get alternative rooms
        [HttpGet]
        public async Task<IActionResult> GetAlternativeRooms(
            int bookingId)
        {
            var rooms =
                await _service.GetAlternativeRoomsAsync(
                    bookingId
                );

            return Json(
                ResponseModel.SuccessResponse(
                    "Success",
                    rooms
                )
            );
        }

        // Propose room change
        [HttpPost]
        public async Task<IActionResult> ProposeRoomChange(
            int bookingId,
            int proposedRoomId,
            string? note)
        {
            var (success, message) =
                await _service.ProposeRoomChangeAsync(
                    bookingId,
                    proposedRoomId,
                    note
                );

            return success
                ? Json(
                    ResponseModel.SuccessResponse(
                        message
                    )
                )
                : Json(
                    ResponseModel.ErrorResponse(
                        message
                    )
                );
        }

        // Send room change email
        [HttpPost]
        public async Task<IActionResult> SendRoomChangeEmail(
            int bookingId)
        {
            var (success, message) =
                await _service.SendRoomChangeEmailAsync(
                    bookingId
                );

            return success
                ? Json(
                    ResponseModel.SuccessResponse(
                        message
                    )
                )
                : Json(
                    ResponseModel.ErrorResponse(
                        message
                    )
                );
        }

        // Show booking details
        [HttpGet]
        public async Task<IActionResult> Details(
            int bookingId)
        {
            var payment =
                await _paymentService.GetByBookingIdAsync(
                    bookingId
                );

            if (payment == null)
            {
                return Content(
                    "<div style='padding:20px;text-align:center;color:#dc2626;'>Payment details not found.</div>"
                );
            }

            return PartialView("Details",payment);
        }

        // Get payment details
        [HttpGet]
        public async Task<IActionResult> GetPaymentDetails(
            int bookingId)
        {
            var payment =
                await _paymentService.GetByBookingIdAsync(
                    bookingId
                );

            if (payment == null)
            {
                return Json(
                    new
                    {
                        success = false,
                        message = "Payment details not found."
                    }
                );
            }

            return Json(
                new
                {
                    success = true,
                    data = new
                    {
                        paymentId = payment.PaymentId,
                        bookingId = payment.BookingId,
                        userId = payment.UserId,
                        amount = payment.Amount,
                        orderId = payment.OrderId,
                        gatewayPaymentId = payment.GatewayPaymentId,
                        paymentStatus = payment.PaymentStatus,
                        paymentDate = payment.PaymentDate,
                        failureReason = payment.FailureReason,
                        createdDate = payment.CreatedDate
                    }
                }
            );
        }
    }
}