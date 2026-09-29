using System;

namespace CMS_HotelBooking.Models
{
    public class AboutCta
    {
        public int AboutCtaId { get; set; }
        public string? Heading { get; set; }
        public string? SubHeading { get; set; }
        public string? ButtonText { get; set; }
        public string? ButtonUrl { get; set; }
        public string? ImagePath { get; set; }
    }
}