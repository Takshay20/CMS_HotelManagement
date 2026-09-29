using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Repository.Interfaces
{
    public interface ISiteSettingRepository
    {
        Task<SiteSetting?> GetAsync();
        Task<int> SaveAsync(SiteSetting model);
    }
}
