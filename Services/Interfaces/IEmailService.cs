using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Services.Interfaces
{
    public interface IEmailService
    {
        Task<string> SendBookingReceivedAsync(Booking booking);
        Task<string> SendBookingStatusUpdateAsync(Booking booking);
        Task<string> SendPaymentCompletedAsync(Booking booking,Payment payment);
        Task<string> SendRoomChangeRequestAsync(Booking booking);
        Task<string> SendBookingCancelledAsync(Booking booking);
        Task<string> SendPasswordResetCodeAsync(string toEmail,string fullName,string resetCode);
        Task<string> SendBookingReminderAsync(Booking booking,string reminderType);
    }
}