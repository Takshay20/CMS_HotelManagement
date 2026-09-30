using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using CMS_HotelBooking.Services.Interfaces;

namespace CMS_HotelBooking.Services.Implementations
{
    public class AboutReceptionService : IAboutReceptionService
    {
        private readonly IAboutReceptionRepository _repository;

        public AboutReceptionService(IAboutReceptionRepository repository)
        {
            _repository = repository;
        }

        // Get all about reception records
        public async Task<List<AboutReception>> GetAllAsync() => await _repository.GetAllAsync();

        // Get about reception record by id
        public async Task<AboutReception?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        // Save about reception record
        public async Task<int> SaveAsync(AboutReception model) => await _repository.SaveAsync(model);

        // Delete about reception record
        public async Task<int> DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }
}
