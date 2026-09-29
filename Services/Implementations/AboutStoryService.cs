using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using CMS_HotelBooking.Services.Interfaces;

namespace CMS_HotelBooking.Services.Implementations
{
    public class AboutStoryService : IAboutStoryService
    {
        private readonly IAboutStoryRepository _repository;

        public AboutStoryService(IAboutStoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<AboutStory>> GetAllAsync() => await _repository.GetAllAsync();

        public async Task<AboutStory?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task<int> SaveAsync(AboutStory model) => await _repository.SaveAsync(model);

        public async Task<int> DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }
}
