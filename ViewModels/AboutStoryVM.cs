using CMS_HotelBooking.Models;
using Microsoft.AspNetCore.Http;

namespace CMS_HotelBooking.ViewModels
{
    public class AboutStoryVM
    {
        public AboutStory AboutStory { get; set; } = new AboutStory();
        public IFormFile? ImageFile { get; set; }
    }
}
