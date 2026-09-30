using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using CMS_HotelBooking.Services.Interfaces;

namespace CMS_HotelBooking.Services.Implementations
{
    public class SocialMediaService : ISocialMediaService
    {
        private readonly ISocialMediaRepository _repository;

        public SocialMediaService(ISocialMediaRepository repository)
        {
            _repository = repository;
        }

        // Get all social media records
        public async Task<List<SocialMedia>> GetAllAsync() => await _repository.GetAllAsync();

        // Get active
        public async Task<List<SocialMedia>> GetActiveAsync() => await _repository.GetActiveAsync();

        // Get social media record by id
        public async Task<SocialMedia?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        // Save social media record
        public async Task<int> SaveAsync(SocialMedia model) => await _repository.SaveAsync(model);

        // Delete social media record
        public async Task<int> DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }
}
