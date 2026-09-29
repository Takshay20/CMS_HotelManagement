using CMS_HotelBooking.Models;
using Microsoft.AspNetCore.Http;

namespace CMS_HotelBooking.ViewModels
{
    public class AboutReceptionVM
    {
        public AboutReception AboutReception { get; set; } = new AboutReception();
        public IFormFile? ImageFile { get; set; }
    }
}
