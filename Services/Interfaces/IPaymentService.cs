using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Services.Interfaces
{
    public interface IPaymentService
    {
        
        Task<(bool Success, string Message, Payment? Payment)> CreatePaymentAsync(
            int bookingId,
            int userId);

        Task<Payment?> GetByBookingIdAsync(int bookingId);

        Task<(bool Success, string Message)> UpdatePaymentStatusAsync(
            int paymentId,
            string paymentStatus,
            string? gatewayPaymentId = null,
            DateTime? paymentDate = null,
            string? failureReason = null);

        Task<(bool Success, string Message)> VerifyAndUpdatePaymentAsync(
            string razorpayPaymentId,
            string razorpayOrderId,
            string razorpaySignature);

        Task<(bool Success, string Message)> UpdatePaymentAmountAsync(
            int paymentId,
            decimal amount);
    }

}