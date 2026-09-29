using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Repository.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment?> GetByBookingIdAsync(int bookingId);

        Task<Payment?> GetByOrderIdAsync(string orderId);

        Task<int> CreateAsync(Payment payment);

        Task<int> UpdatePaymentOrderAsync(
            int paymentId,
            decimal amount,
            string orderId,
            string paymentStatus);

        Task<int> UpdateStatusAsync(
            int paymentId,
            string paymentStatus,
            string? gatewayPaymentId,
            DateTime? paymentDate,
            string? failureReason);

        Task<Payment?> GetByPaymentIdAsync(int paymentId);
        Task<int> UpdatePaymentAmountAsync(int paymentId, decimal amount);
    }
}