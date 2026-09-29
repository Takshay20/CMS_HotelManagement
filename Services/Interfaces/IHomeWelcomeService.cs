using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Services.Interfaces
{
    public interface IHomeWelcomeService
    {
        Task<HomeWelcome?> GetAsync();

        Task<int> UpdateAsync(HomeWelcome model);
    }
}