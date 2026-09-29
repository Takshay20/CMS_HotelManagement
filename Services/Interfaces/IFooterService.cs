using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Services.Interfaces
{
    public interface IFooterService
    {
        Task<Footer?> GetAsync();
        Task<int> SaveAsync(Footer model);
    }
}
