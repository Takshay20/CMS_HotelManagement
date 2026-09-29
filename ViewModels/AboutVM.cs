using CMS_HotelBooking.Models;
using Microsoft.AspNetCore.Http;

namespace CMS_HotelBooking.ViewModels
{
    public class AboutVM
    {
        public About About { get; set; } = new About();
        public IFormFile? Image1File { get; set; }
    }
}
