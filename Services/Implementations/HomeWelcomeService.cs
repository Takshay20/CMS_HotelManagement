using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using CMS_HotelBooking.Services.Interfaces;

namespace CMS_HotelBooking.Services.Implementations
{
    public class HomeWelcomeService : IHomeWelcomeService
    {
        private readonly IHomeWelcomeRepository _repository;

        public HomeWelcomeService(IHomeWelcomeRepository repository)
        {
            _repository = repository;
        }

        // Get home welcome record
        public async Task<HomeWelcome?> GetAsync()
        {
            return await _repository.GetAsync();
        }

        // Update home welcome record
        public async Task<int> UpdateAsync(HomeWelcome model)
        {
            return await _repository.UpdateAsync(model);
        }
    }
}