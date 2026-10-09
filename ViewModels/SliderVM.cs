using CMS_HotelBooking.Models;
using Microsoft.AspNetCore.Http;

namespace CMS_HotelBooking.ViewModels
{
    public class SliderVM
    {
        public Slider Slider { get; set; } = new Slider();
        public IFormFile? ImageFile { get; set; }
    }
}