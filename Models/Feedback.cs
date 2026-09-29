using System;

namespace CMS_HotelBooking.Models
{
    public class Feedback
    {
        public int FeedbackId { get; set; }
        public int? UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Designation { get; set; }
        public string? ImagePath { get; set; }
        public string Message { get; set; } = string.Empty;
        public int Rating { get; set; } = 5;
        public bool IsApproved { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
