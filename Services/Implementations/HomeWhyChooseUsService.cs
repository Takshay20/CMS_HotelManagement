using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using CMS_HotelBooking.Services.Interfaces;

namespace CMS_HotelBooking.Services.Implementations
{
    public class HomeWhyChooseUsService : IHomeWhyChooseUsService
    {
        private readonly IHomeWhyChooseUsRepository _repository;

        public HomeWhyChooseUsService(IHomeWhyChooseUsRepository repository)
        {
            _repository = repository;
        }

        // Get all home why choose us records
        public async Task<List<HomeWhyChooseUs>> GetAllAsync() => await _repository.GetAllAsync();

        // Get all records
        public async Task<IEnumerable<HomeWhyChooseUs>> GetAllRecordsAsync(string filter)
        {
            return await _repository.GetAllRecordsAsync(filter);
        }

        // Get home why choose us record by id
        public async Task<HomeWhyChooseUs?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        // Save home why choose us record
        public async Task<int> SaveAsync(HomeWhyChooseUs model) => await _repository.SaveAsync(model);

        // Delete home why choose us record
        public async Task<int> DeleteAsync(int id) => await _repository.DeleteAsync(id);

        // Restore deleted home why choose us record
        public async Task<int> RestoreAsync(int id)
        {
            return await _repository.RestoreAsync(id);
        }

    }
}
