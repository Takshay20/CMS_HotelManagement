using System;

namespace CMS_HotelBooking.Models
{
    public class BookingReminder
    {
        public int ReminderId { get; set; }

        public int BookingId { get; set; }

        public string ReminderType { get; set; } = string.Empty;

        public DateTime? SentDate { get; set; }

        public bool IsSent { get; set; }
    }
}