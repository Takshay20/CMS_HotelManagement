using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using CMS_HotelBooking.Services.Interfaces;

namespace CMS_HotelBooking.Services.Implementations
{
    public class AboutService : IAboutService
    {
        private readonly IAboutRepository _repository;

        public AboutService(IAboutRepository repository)
        {
            _repository = repository;
        }

        // Get about record
        public async Task<About?> GetAsync() => await _repository.GetAsync();

        // Save about record
        public async Task<int> SaveAsync(About model) => await _repository.SaveAsync(model);
    }
}
