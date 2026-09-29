using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Repository.Interfaces
{
    public interface IHomeWelcomeRepository
    {
        Task<HomeWelcome?> GetAsync();

        Task<int> UpdateAsync(HomeWelcome model);
    }
}