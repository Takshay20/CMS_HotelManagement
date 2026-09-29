using CMS_HotelBooking.Models;
using Microsoft.AspNetCore.Http;

namespace CMS_HotelBooking.ViewModels
{
    public class HomeWelcomeVM
    {
        public HomeWelcome HomeWelcome { get; set; } = new HomeWelcome();

        public IFormFile? Image1File { get; set; }

        public IFormFile? Image2File { get; set; }

        public IFormFile? Image3File { get; set; }
    }
}