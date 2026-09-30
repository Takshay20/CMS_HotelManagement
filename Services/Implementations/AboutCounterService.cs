using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using CMS_HotelBooking.Services.Interfaces;

namespace CMS_HotelBooking.Services.Implementations
{
    public class AboutCounterService : IAboutCounterService
    {
        private readonly IAboutCounterRepository _repository;

        public AboutCounterService(IAboutCounterRepository repository)
        {
            _repository = repository;
        }

        // Get all about counter records
        public async Task<List<AboutCounter>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        // Get all records
        public async Task<List<AboutCounter>> GetAllRecordsAsync(string filter)
        {
            return await _repository.GetAllRecordsAsync(filter);
        }

        // Get about counter record by id
        public async Task<AboutCounter?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        // Save about counter record
        public async Task<int> SaveAsync(AboutCounter model)
        {
            return await _repository.SaveAsync(model);
        }

        // Delete about counter record
        public async Task<int> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        // Restore deleted about counter record
        public async Task<int> RestoreAsync(int id)
        {
            return await _repository.RestoreAsync(id);
        }

        // Get deleted date
        public async Task<DateTime?> GetDeletedDateAsync(int id)
        {
            return await _repository.GetDeletedDateAsync(id);
        }
    }
}