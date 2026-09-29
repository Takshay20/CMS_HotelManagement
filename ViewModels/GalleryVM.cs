using CMS_HotelBooking.Models;
using Microsoft.AspNetCore.Http;

namespace CMS_HotelBooking.ViewModels
{
    public class GalleryVM
    {
        public Gallery Gallery { get; set; } = new Gallery();
        public IFormFile? ImageFile { get; set; }
    }
}
