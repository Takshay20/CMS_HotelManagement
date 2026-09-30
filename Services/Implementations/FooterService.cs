using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using CMS_HotelBooking.Services.Interfaces;

namespace CMS_HotelBooking.Services.Implementations
{
    public class FooterService : IFooterService
    {
        private readonly IFooterRepository _repository;

        public FooterService(IFooterRepository repository)
        {
            _repository = repository;
        }

        // Get footer record
        public async Task<Footer?> GetAsync() => await _repository.GetAsync();

        // Save footer record
        public async Task<int> SaveAsync(Footer model) => await _repository.SaveAsync(model);
    }
}
