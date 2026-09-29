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

        public async Task<List<SocialMedia>> GetAllAsync() => await _repository.GetAllAsync();

        public async Task<List<SocialMedia>> GetActiveAsync() => await _repository.GetActiveAsync();

        public async Task<SocialMedia?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task<int> SaveAsync(SocialMedia model) => await _repository.SaveAsync(model);

        public async Task<int> DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }
}
