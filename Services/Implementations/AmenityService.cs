using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using CMS_HotelBooking.Services.Interfaces;

namespace CMS_HotelBooking.Services.Implementations
{
    public class AmenityService : IAmenityService
    {
        private readonly IAmenityRepository _repository;

        public AmenityService(IAmenityRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Amenity>> GetAllAsync() => await _repository.GetAllAsync();

        public async Task<Amenity?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task<int> SaveAsync(Amenity model) => await _repository.SaveAsync(model);

        public async Task<int> DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }
}
