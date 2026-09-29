using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Services.Interfaces
{
    public interface ISiteSettingService
    {
        Task<SiteSetting?> GetAsync();
        Task<int> SaveAsync(SiteSetting model);
    }
}
