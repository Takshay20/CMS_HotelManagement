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

        // Get all about story records
        public async Task<List<AboutStory>> GetAllAsync() => await _repository.GetAllAsync();

        // Get about story record by id
        public async Task<AboutStory?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        // Save about story record
        public async Task<int> SaveAsync(AboutStory model) => await _repository.SaveAsync(model);



        
    }
}
