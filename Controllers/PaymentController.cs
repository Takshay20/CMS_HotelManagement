using CMS_HotelBooking.Models;
using CMS_HotelBooking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Razorpay.Api;

namespace CMS_HotelBooking.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly RazorpaySettings _razorpaySettings;

        public PaymentController(IPaymentService paymentService,IOptions<RazorpaySettings> razorpayOptions)
        {
            _paymentService = paymentService;
            _razorpaySettings = razorpayOptions.Value;
        }

        // Open payment page
        [HttpGet]
        public async Task<IActionResult> Pay(int id)
        {
            var userIdClaim =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Content("User ID not found. Please login again.");
            }

            var result =
                await _paymentService.CreatePaymentAsync(id, userId);

            if (!result.Success && result.Payment != null)
            {
                ViewBag.RazorpayKey = _razorpaySettings.KeyId;
                ViewBag.OrderId = result.Payment.OrderId;
                ViewBag.Amount = result.Payment.Amount;
                ViewBag.PaymentId = result.Payment.PaymentId;
                ViewBag.BookingId = result.Payment.BookingId;
                ViewBag.Message = result.Message;

                return View(result.Payment);
            }

            if (!result.Success || result.Payment == null)
            {
                return Content(
                    $"Payment failed for BookingId: {id}\n\n" +
                    $"UserId: {userId}\n\n" +
                    $"Message: {result.Message}"
                );
            }
            ViewBag.RazorpayKey = _razorpaySettings.KeyId;
            ViewBag.OrderId = result.Payment.OrderId;
            ViewBag.Amount = result.Payment.Amount;
            ViewBag.PaymentId = result.Payment.PaymentId;
            ViewBag.BookingId = result.Payment.BookingId;

            return View(result.Payment);
        }

        // Show payment details
        [HttpGet]
        public async Task<IActionResult> Details(int bookingId)
        {
            var payment = await _paymentService.GetByBookingIdAsync(bookingId);

            if (payment == null)
            {
                TempData["Error"] ="Payment record not found.";
                return RedirectToAction("MyBookings","Booking");
            }

            return View(payment);
        }

        // Verify payment
        [HttpPost]
        public async Task<IActionResult> VerifyPayment(string razorpay_payment_id, string razorpay_order_id, string razorpay_signature)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(razorpay_payment_id) ||
                    string.IsNullOrWhiteSpace(razorpay_order_id) ||
                    string.IsNullOrWhiteSpace(razorpay_signature))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Invalid payment response."
                    });
                }

                var attributes = new Dictionary<string, string>
        {
            {
                "razorpay_order_id",
                razorpay_order_id
            },
            {
                "razorpay_payment_id",
                razorpay_payment_id
            },
            {
                "razorpay_signature",
                razorpay_signature
            }
        };

                Razorpay.Api.Utils.verifyPaymentSignature(
                    attributes
                );

                var result =
                    await _paymentService.VerifyAndUpdatePaymentAsync(
                        razorpay_payment_id,
                        razorpay_order_id,
                        razorpay_signature
                    );

                if (!result.Success)
                {
                    return Json(new
                    {
                        success = false,
                        message = result.Message
                    });
                }

                return Json(new
                {
                    success = true,
                    message = result.Message,
                    redirectUrl = Url.Action("MyBookings", "Booking")
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Payment verification failed: "+ ex.Message
                });
            }
        }
    }
}