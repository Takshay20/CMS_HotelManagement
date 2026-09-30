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

        // Get all amenity records
        public async Task<List<Amenity>> GetAllAsync() => await _repository.GetAllAsync();

        // Get amenity record by id
        public async Task<Amenity?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        // Save amenity record
        public async Task<int> SaveAsync(Amenity model) => await _repository.SaveAsync(model);

        // Delete amenity record
        public async Task<int> DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }
}
