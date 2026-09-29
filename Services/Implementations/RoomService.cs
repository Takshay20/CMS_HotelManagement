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

        public async Task<List<Room>> GetAllAsync() => await _repository.GetAllAsync();

        public async Task<Room?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task<List<Room>> GetFeaturedAsync(int top = 6) => await _repository.GetFeaturedAsync(top);

        public async Task<int> SaveAsync(Room model) => await _repository.SaveAsync(model);

        public async Task<int> DeleteAsync(int id) => await _repository.DeleteAsync(id);

        public async Task<int> AddImageAsync(int roomId, string imagePath, int displayOrder) =>
            await _repository.AddImageAsync(roomId, imagePath, displayOrder);

        public async Task<int> DeleteImageAsync(int roomImageId) => await _repository.DeleteImageAsync(roomImageId);
    }
}
