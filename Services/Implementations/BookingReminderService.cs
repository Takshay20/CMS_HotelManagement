using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using CMS_HotelBooking.Services.Interfaces;

namespace CMS_HotelBooking.Services.Implementations
{
    public class BookingReminderService : IBookingReminderService
    {
        private readonly IBookingReminderRepository _reminderRepository;
        private readonly IEmailService _emailService;

        public BookingReminderService(IBookingReminderRepository reminderRepository,IEmailService emailService)
        {
            _reminderRepository = reminderRepository;
            _emailService = emailService;
        }

        // Process reminders
        public async Task ProcessRemindersAsync()
        {
            var bookings = await _reminderRepository.GetBookingsForReminderAsync();

            if (bookings == null || bookings.Count == 0)
                return;

            var now = DateTime.Now;

            foreach (var booking in bookings)
            {
                try
                {
                    var checkInDateTime =booking.CheckInDate.Date.AddHours(11);

                    await ProcessReminderAsync(booking,checkInDateTime.AddHours(-24),"24H",now);

                    await ProcessReminderAsync( booking,checkInDateTime.AddHours(-12),"12H",now);

                    await ProcessReminderAsync(booking,checkInDateTime.AddHours(-6),"6H",now);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        $"Booking reminder error for BookingId {booking.BookingId}: {ex.Message}"
                    );
                }
            }
        }

        // Process reminder
        private async Task ProcessReminderAsync(Booking booking,DateTime reminderTime,string reminderType,DateTime now)
        {
            if (now < reminderTime)
                return;

            if (now >= reminderTime.AddHours(1))
                return;

            var existingReminder =
                await _reminderRepository.GetByBookingAndTypeAsync(booking.BookingId,reminderType);

            if (existingReminder != null && existingReminder.IsSent)
            {
                return;
            }

            var emailStatus =await _emailService.SendBookingReminderAsync(booking,reminderType);

            if (emailStatus == "Sent")
            {
                var reminder = new BookingReminder
                {
                    BookingId = booking.BookingId,
                    ReminderType = reminderType,
                    SentDate = DateTime.Now,
                    IsSent = true
                };

                await _reminderRepository.CreateAsync(reminder);

                Console.WriteLine($"{reminderType} reminder sent for BookingId {booking.BookingId}");
            }
            else
            {
                Console.WriteLine($"{reminderType} reminder failed for BookingId {booking.BookingId}. Email Status: {emailStatus}");
            }
        }
    }
}