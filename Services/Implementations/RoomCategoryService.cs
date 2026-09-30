using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using CMS_HotelBooking.Services.Interfaces;

namespace CMS_HotelBooking.Services.Implementations
{
    public class RoomCategoryService : IRoomCategoryService
    {
        private readonly IRoomCategoryRepository _repository;

        public RoomCategoryService(IRoomCategoryRepository repository)
        {
            _repository = repository;
        }

        // Get all room category records
        public async Task<List<RoomCategory>> GetAllAsync() => await _repository.GetAllAsync();

        // Get room category record by id
        public async Task<RoomCategory?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        // Save room category record
        public async Task<int> SaveAsync(RoomCategory model) => await _repository.SaveAsync(model);

        // Delete room category record
        public async Task<int> DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }
}
