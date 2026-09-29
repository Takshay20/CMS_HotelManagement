using System;

namespace CMS_HotelBooking.Models
{
    public class AboutStory
    {
        public int AboutStoryId { get; set; }
        public string Heading { get; set; } = string.Empty;
        public string? SubHeading { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; }
    }
}
