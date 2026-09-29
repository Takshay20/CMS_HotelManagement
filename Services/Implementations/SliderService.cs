using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using CMS_HotelBooking.Services.Interfaces;

namespace CMS_HotelBooking.Services.Implementations
{
    public class SliderService : ISliderService
    {
        private readonly ISliderRepository _repository;

        public SliderService(ISliderRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Slider>> GetAllAsync(string? pageKey = null)
        {
            return await _repository.GetAllAsync(pageKey);
        }

        public async Task<Slider?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<int> SaveAsync(Slider model)
        {
            return await _repository.SaveAsync(model);
        }

        public async Task<int> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}