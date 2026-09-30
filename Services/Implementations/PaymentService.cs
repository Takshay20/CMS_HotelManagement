using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using CMS_HotelBooking.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace CMS_HotelBooking.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IEmailService _emailService;
        private readonly RazorpaySettings _razorpaySettings;

        public PaymentService(
            IPaymentRepository paymentRepository,
            IBookingRepository bookingRepository,
            IEmailService emailService,
            IOptions<RazorpaySettings> razorpayOptions)
        {
            _paymentRepository = paymentRepository;
            _bookingRepository = bookingRepository;
            _emailService = emailService;
            _razorpaySettings = razorpayOptions.Value;
        }

        public async Task<(bool Success, string Message, Payment? Payment)>
            CreatePaymentAsync(
                int bookingId,
                int userId)
        {
            var booking =
                await _bookingRepository.GetByIdAsync(bookingId);

            if (booking == null)
            {
                return (
                    false,
                    "Booking not found.",
                    null
                );
            }

            if (!string.Equals(
                    booking.Status,
                    "Approved",
                    StringComparison.OrdinalIgnoreCase))
            {
                return (
                    false,
                    "Only approved bookings can proceed to payment.",
                    null
                );
            }

            if (booking.UserId != userId)
            {
                return (
                    false,
                    "You are not authorized to pay for this booking.",
                    null
                );
            }

            var existingPayment =
                await _paymentRepository.GetByBookingIdAsync(
                    bookingId
                );

            if (existingPayment != null &&
                string.Equals(
                    existingPayment.PaymentStatus,
                    "Paid",
                    StringComparison.OrdinalIgnoreCase))
            {
                return (
                    false,
                    "Payment has already been completed for this booking.",
                    existingPayment
                );
            }

            try
            {
                decimal totalAmount = booking.TotalPrice;

                if (totalAmount <= 0)
                {
                    return (
                        false,
                        "Invalid payment amount.",
                        null
                    );
                }

                long amountInPaise =
                    Convert.ToInt64(
                        Math.Round(
                            totalAmount * 100,
                            MidpointRounding.AwayFromZero
                        )
                    );

                if (amountInPaise <= 0)
                {
                    return (
                        false,
                        "Invalid payment amount.",
                        null
                    );
                }

                var client =
                    new Razorpay.Api.RazorpayClient(
                        _razorpaySettings.KeyId,
                        _razorpaySettings.KeySecret
                    );

                var options =
                    new Dictionary<string, object>
                    {
                        {
                            "amount",
                            amountInPaise
                        },
                        {
                            "currency",
                            _razorpaySettings.Currency
                        },
                        {
                            "receipt",
                            $"booking_{booking.BookingId}"
                        },
                        {
                            "payment_capture",
                            1
                        }
                    };

                Razorpay.Api.Order order =
                    client.Order.Create(options);

                string razorpayOrderId =
                    order["id"]?.ToString()
                    ?? string.Empty;

                if (string.IsNullOrWhiteSpace(
                        razorpayOrderId))
                {
                    return (
                        false,
                        "Razorpay order could not be created.",
                        null
                    );
                }

                if (existingPayment != null)
                {
                    existingPayment.Amount =
                        booking.TotalPrice;

                    existingPayment.OrderId =
                        razorpayOrderId;

                    existingPayment.PaymentStatus =
                        "Pending";

                    existingPayment.GatewayPaymentId =
                        null;

                    existingPayment.PaymentDate =
                        null;

                    existingPayment.FailureReason =
                        null;

                    var updateResult =
                        await _paymentRepository.UpdatePaymentOrderAsync(
                            existingPayment.PaymentId,
                            existingPayment.Amount,
                            existingPayment.OrderId,
                            existingPayment.PaymentStatus
                        );

                    if (updateResult <= 0)
                    {
                        return (
                            false,
                            "Unable to update existing payment record.",
                            null
                        );
                    }

                    return (
                        true,
                        "Razorpay order created successfully.",
                        existingPayment
                    );
                }

                var payment =
                    new Payment
                    {
                        BookingId =
                            booking.BookingId,

                        UserId =
                            userId,

                        Amount =
                            booking.TotalPrice,

                        OrderId =
                            razorpayOrderId,

                        PaymentStatus =
                            "Pending",

                        CreatedDate =
                            DateTime.Now
                    };

                var paymentId =
                    await _paymentRepository.CreateAsync(
                        payment
                    );

                if (paymentId <= 0)
                {
                    return (
                        false,
                        "Payment record could not be saved.",
                        null
                    );
                }

                payment.PaymentId =
                    paymentId;

                return (
                    true,
                    "Razorpay order created successfully.",
                    payment
                );
            }
            catch (Exception ex)
            {
                return (
                    false,
                    $"Unable to create Razorpay order: {ex.Message}",
                    null
                );
            }
        }

        // Get by booking id
        public async Task<Payment?> GetByBookingIdAsync(
            int bookingId)
        {
            return await _paymentRepository
                .GetByBookingIdAsync(bookingId);
        }

        public async Task<(bool Success, string Message)>
            UpdatePaymentStatusAsync(
                int paymentId,
                string paymentStatus,
                string? gatewayPaymentId = null,
                DateTime? paymentDate = null,
                string? failureReason = null)
        {
            var payment =
                await GetPaymentByIdAsync(
                    paymentId
                );

            if (payment == null)
            {
                return (
                    false,
                    "Payment record not found."
                );
            }

            var allowedStatuses =
                new[]
                {
                    "Pending",
                    "Paid",
                    "Failed",
                    "Refunded"
                };

            if (!allowedStatuses.Contains(
                    paymentStatus,
                    StringComparer.OrdinalIgnoreCase))
            {
                return (
                    false,
                    "Invalid payment status."
                );
            }

            var result =
                await _paymentRepository.UpdateStatusAsync(
                    paymentId,
                    paymentStatus,
                    gatewayPaymentId,
                    paymentDate,
                    failureReason
                );

            if (result <= 0)
            {
                return (
                    false,
                    "Unable to update payment status."
                );
            }

            return (
                true,
                $"Payment status updated to {paymentStatus}."
            );
        }

        public async Task<(bool Success, string Message)>
    VerifyAndUpdatePaymentAsync(
        string razorpayPaymentId,
        string razorpayOrderId,
        string razorpaySignature)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(razorpayPaymentId) ||
                    string.IsNullOrWhiteSpace(razorpayOrderId) ||
                    string.IsNullOrWhiteSpace(razorpaySignature))
                {
                    return (
                        false,
                        "Invalid payment details."
                    );
                }

                var payment =
                    await _paymentRepository.GetByOrderIdAsync(
                        razorpayOrderId
                    );

                if (payment == null)
                {
                    return (
                        false,
                        "Payment record not found."
                    );
                }

                if (string.Equals(
                        payment.PaymentStatus,
                        "Paid",
                        StringComparison.OrdinalIgnoreCase))
                {
                    return (
                        true,
                        "Payment has already been completed."
                    );
                }

                var result =
                    await _paymentRepository.UpdateStatusAsync(
                        payment.PaymentId,
                        "Paid",
                        razorpayPaymentId,
                        DateTime.Now,
                        null
                    );

                if (result <= 0)
                {
                    return (
                        false,
                        "Unable to update payment status."
                    );
                }

                payment.PaymentStatus = "Paid";
                payment.GatewayPaymentId = razorpayPaymentId;
                payment.PaymentDate = DateTime.Now;

                var booking =
                    await _bookingRepository.GetByIdAsync(
                        payment.BookingId
                    );

                if (booking != null)
                {
                    booking.PaymentStatus = "Paid";

                    try
                    {
                        await _emailService.SendPaymentCompletedAsync(
                            booking,
                            payment
                        );
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(
                            "Payment Email Error: " +
                            ex.Message
                        );
                    }
                }

                return (
                    true,
                    "Payment completed successfully."
                );
            }
            catch (Exception ex)
            {
                return (
                    false,
                    "Payment verification failed: " +
                    ex.Message
                );
            }
        }

        // Get payment by id
        private async Task<Payment?> GetPaymentByIdAsync(
            int paymentId)
        {
            return await _paymentRepository
                .GetByPaymentIdAsync(
                    paymentId
                );
        }

        public async Task<(bool Success, string Message)> UpdatePaymentAmountAsync(
    int paymentId,
    decimal amount)
        {
            if (amount <= 0)
            {
                return (false, "Invalid payment amount.");
            }

            var payment =
                await _paymentRepository.GetByPaymentIdAsync(
                    paymentId
                );

            if (payment == null)
            {
                return (false, "Payment record not found.");
            }

            var result =
                await _paymentRepository.UpdatePaymentAmountAsync(
                    paymentId,
                    amount
                );

            if (result <= 0)
            {
                return (false, "Unable to update payment amount.");
            }

            return (
                true,
                "Payment amount updated successfully."
            );
        }
    }
}