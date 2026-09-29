using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Services.Interfaces
{
    public interface IBookingService
    {
        Task<List<Booking>> GetAllAsync();
        Task<Booking?> GetByIdAsync(int id);
        Task<List<Booking>> GetByUserIdAsync(int userId);
        Task<(bool Success, string Message)> CreateAsync(Booking model);
        Task<int> UpdateStatusAsync(int id, string status);
        Task<(int Result, string EmailNote)> UpdateStatusWithEmailAsync(int id, string status);
        Task<int> DeleteAsync(int id);

        Task<(bool Available, string Message)> CheckAvailabilityAsync(int roomId, DateTime checkIn, DateTime checkOut, int? excludeBookingId = null);
        Task<List<Room>> GetAlternativeRoomsAsync(int bookingId);
        Task<(bool Success, string Message)> ProposeRoomChangeAsync(int bookingId, int proposedRoomId, string? note);

        Task<(bool Success, string Message)> SendRoomChangeEmailAsync(int bookingId);
        Task<(bool Success, string Message)> RespondRoomChangeAsync(int bookingId, int userId, bool accept);

        Task<(bool Success, string Message, int RefundPercentage, decimal RefundAmount)> CancelByGuestAsync(int bookingId, int userId);
    }
}
