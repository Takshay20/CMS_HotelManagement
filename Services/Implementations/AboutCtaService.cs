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

        // Get all about cta records
        public async Task<List<AboutCta>> GetAllAsync() => await _repository.GetAllAsync();

        // Get about cta record by id
        public async Task<AboutCta?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        // Save about cta record
        public async Task<int> SaveAsync(AboutCta model) => await _repository.SaveAsync(model);

        // Delete about cta record
        public async Task<int> DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }
}
