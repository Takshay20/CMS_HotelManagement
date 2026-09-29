using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using CMS_HotelBooking.Services.Interfaces;

namespace CMS_HotelBooking.Services.Implementations
{
    public class AboutCtaService : IAboutCtaService
    {
        private readonly IAboutCtaRepository _repository;

        public AboutCtaService(IAboutCtaRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<AboutCta>> GetAllAsync() => await _repository.GetAllAsync();

        public async Task<AboutCta?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task<int> SaveAsync(AboutCta model) => await _repository.SaveAsync(model);

        public async Task<int> DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }
}
