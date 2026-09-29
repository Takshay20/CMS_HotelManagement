using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Services.Interfaces
{
    public interface IRoomCategoryService
    {
        Task<List<RoomCategory>> GetAllAsync();
        Task<RoomCategory?> GetByIdAsync(int id);
        Task<int> SaveAsync(RoomCategory model);
        Task<int> DeleteAsync(int id);
    }
}
