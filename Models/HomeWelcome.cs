using System;

namespace CMS_HotelBooking.Models
{
    public class HomeWelcome
    {
        public int HomeWelcomeId { get; set; }
        public string Heading { get; set; } = string.Empty;
        public string? SubHeading { get; set; }
        public string? Description { get; set; }
        public string? Image1Path { get; set; }
        public string? Image2Path { get; set; }
        public string? Image3Path { get; set; }
        public string? ButtonText { get; set; }
        public string? ButtonUrl { get; set; }
        public int? ExperienceYears { get; set; }
        public int DisplayOrder { get; set; } = 1;
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}