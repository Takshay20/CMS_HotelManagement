using System;
namespace CMS_HotelBooking.Models
{
    public class AboutCounter
    {
        public int AboutCounterId { get; set; }
        public string? IconClass { get; set; }
        public string Number { get; set; } = string.Empty;
        public string? Suffix { get; set; }
        public string Label { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}