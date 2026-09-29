using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Repository.Interfaces
{
    public interface IBookingRepository
    {
        Task<List<Booking>> GetAllAsync();
        Task<Booking?> GetByIdAsync(int id);
        Task<List<Booking>> GetByUserIdAsync(int userId);
        Task<int> CreateAsync(Booking model);
        Task<int> UpdateStatusAsync(int id, string status);
        Task<int> DeleteAsync(int id);

        Task<int> GetConflictCountAsync(int roomId, DateTime checkIn, DateTime checkOut, int? excludeBookingId = null);
        Task<List<Room>> GetRoomsByCategoryAsync(int roomCategoryId, int? excludeRoomId = null);
        Task<int> ProposeRoomChangeAsync(int bookingId, int proposedRoomId, string? note);
        Task<int> RespondRoomChangeAsync(int bookingId, bool accept);
        Task<int> UpdateEmailStatusAsync(int bookingId, string emailStatus, string emailType);
        Task<int> CancelWithRefundAsync(int bookingId, int refundPercentage, decimal refundAmount, string cancelledBy);
    }
}
