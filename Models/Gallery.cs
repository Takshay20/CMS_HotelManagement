using System;

namespace CMS_HotelBooking.Models
{
    public class Gallery
    {
        public int GalleryId { get; set; }
        public string? Title { get; set; }
        public string ImagePath { get; set; } = string.Empty;
        public string? Category { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; }
    }
}
