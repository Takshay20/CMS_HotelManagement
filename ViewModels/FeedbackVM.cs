using Microsoft.AspNetCore.Http;

namespace CMS_HotelBooking.ViewModels
{
    public class FeedbackVM
    {
        public string Name { get; set; } = string.Empty;
        public string? Designation { get; set; }
        public string Message { get; set; } = string.Empty;
        public int Rating { get; set; } = 5;
        public IFormFile? ImageFile { get; set; }
    }
}
