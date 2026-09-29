using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Repository.Interfaces
{
    public interface IBookingReminderRepository
    {
        
        Task<BookingReminder?> GetByBookingAndTypeAsync(
            int bookingId,
            string reminderType);

        Task<int> CreateAsync(
            BookingReminder reminder);

        Task<List<Booking>> GetBookingsForReminderAsync();
    }
}