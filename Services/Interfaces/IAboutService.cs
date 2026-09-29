using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Services.Interfaces
{
    public interface IAboutService
    {
        Task<About?> GetAsync();
        Task<int> SaveAsync(About model);
    }
}
