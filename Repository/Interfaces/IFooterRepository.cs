using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Repository.Interfaces
{
    public interface IFooterRepository
    {
        Task<Footer?> GetAsync();
        Task<int> SaveAsync(Footer model);
    }
}
