using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using CMS_HotelBooking.Services.Interfaces;

namespace CMS_HotelBooking.Services.Implementations
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _repository;

        public RoomService(IRoomRepository repository)
        {
            _repository = repository;
        }

        // Get all room records
        public async Task<List<Room>> GetAllAsync() => await _repository.GetAllAsync();

        // Get room record by id
        public async Task<Room?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        // Get featured
        public async Task<List<Room>> GetFeaturedAsync(int top = 6) => await _repository.GetFeaturedAsync(top);

        // Save room record
        public async Task<int> SaveAsync(Room model) => await _repository.SaveAsync(model);

        // Delete room record
        public async Task<int> DeleteAsync(int id) => await _repository.DeleteAsync(id);

        // Add image
        public async Task<int> AddImageAsync(int roomId, string imagePath, int displayOrder) =>
            await _repository.AddImageAsync(roomId, imagePath, displayOrder);

        // Delete image
        public async Task<int> DeleteImageAsync(int roomImageId) => await _repository.DeleteImageAsync(roomImageId);
    }
}
