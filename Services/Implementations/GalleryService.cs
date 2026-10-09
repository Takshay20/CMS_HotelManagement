using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using CMS_HotelBooking.Services.Interfaces;

namespace CMS_HotelBooking.Services.Implementations
{
    public class GalleryService : IGalleryService
    {
        private readonly IGalleryRepository _repository;

        public GalleryService(IGalleryRepository repository)
        {
            _repository = repository;
        }

        // Get all gallery records
        public async Task<List<Gallery>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        // Get gallery record by id
        public async Task<Gallery?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        // Save gallery record
        public async Task<int> SaveAsync(Gallery model)
        {
            return await _repository.SaveAsync(model);
        }

        // Delete gallery record
        public async Task<int> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}