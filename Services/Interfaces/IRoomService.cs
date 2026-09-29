using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Services.Interfaces
{
    public interface IRoomService
    {
        Task<List<Room>> GetAllAsync();
        Task<Room?> GetByIdAsync(int id);
        Task<List<Room>> GetFeaturedAsync(int top = 6);
        Task<int> SaveAsync(Room model);
        Task<int> DeleteAsync(int id);
        Task<int> AddImageAsync(int roomId, string imagePath, int displayOrder);
        Task<int> DeleteImageAsync(int roomImageId);
    }
}
