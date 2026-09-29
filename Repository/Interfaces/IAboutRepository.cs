using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Repository.Interfaces
{
    public interface IAboutRepository
    {
        Task<About?> GetAsync();
        Task<int> SaveAsync(About model);
    }
}
