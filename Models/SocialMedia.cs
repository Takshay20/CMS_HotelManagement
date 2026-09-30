using System;

namespace CMS_HotelBooking.Models
{
    public class SocialMedia
    {
        public int SocialMediaId { get; set; }
        public string PlatformName { get; set; } = string.Empty;
        public string? IconClass { get; set; }
        public string Url { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
