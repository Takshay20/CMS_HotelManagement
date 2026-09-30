using System;

namespace CMS_HotelBooking.Models
{
    public class Slider
    {
        public int SliderId { get; set; }
        public string PageKey { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string? ButtonText { get; set; }
        public string? ButtonUrl { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
