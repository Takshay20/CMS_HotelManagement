using CMS_HotelBooking.Models;
using Microsoft.AspNetCore.Http;

namespace CMS_HotelBooking.ViewModels
{
    public class FacilityVM
    {
        public Facility Facility { get; set; } = new Facility();
        public IFormFile? ImageFile { get; set; }
    }
}
