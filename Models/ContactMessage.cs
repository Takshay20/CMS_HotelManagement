using System;

namespace CMS_HotelBooking.Models
{
    public class ContactMessage
    {
        public int ContactMessageId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Subject { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? AdminReply { get; set; }
        public DateTime? RepliedDate { get; set; }
    }
}
