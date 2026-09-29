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

        public async Task<List<AboutCounter>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<List<AboutCounter>> GetAllRecordsAsync(string filter)
        {
            return await _repository.GetAllRecordsAsync(filter);
        }

        public async Task<AboutCounter?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<int> SaveAsync(AboutCounter model)
        {
            return await _repository.SaveAsync(model);
        }

        public async Task<int> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<int> RestoreAsync(int id)
        {
            return await _repository.RestoreAsync(id);
        }
        public async Task<DateTime?> GetDeletedDateAsync(int id)
        {
            return await _repository.GetDeletedDateAsync(id);
        }
    }
}