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

        public async Task<List<RoomCategory>> GetAllAsync() => await _repository.GetAllAsync();

        public async Task<RoomCategory?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task<int> SaveAsync(RoomCategory model) => await _repository.SaveAsync(model);

        public async Task<int> DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }
}
