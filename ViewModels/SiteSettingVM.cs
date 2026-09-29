using CMS_HotelBooking.Models;
using Microsoft.AspNetCore.Http;

namespace CMS_HotelBooking.ViewModels
{
    public class SiteSettingVM
    {
        public SiteSetting SiteSetting { get; set; } = new SiteSetting();
        public IFormFile? LogoFile { get; set; }
    }
}
