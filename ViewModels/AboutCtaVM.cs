using CMS_HotelBooking.Models;
using Microsoft.AspNetCore.Http;

namespace CMS_HotelBooking.ViewModels
{
    public class AboutCtaVM
    {
        public AboutCta AboutCta { get; set; } = new AboutCta();
        public IFormFile? ImageFile { get; set; }
    }
}
