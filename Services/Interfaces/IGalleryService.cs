using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Services.Interfaces
{
    public interface IGalleryService
    {
        Task<List<Gallery>> GetAllAsync();
        Task<Gallery?> GetByIdAsync(int id);
        Task<int> SaveAsync(Gallery model);
        Task<int> DeleteAsync(int id);
    }
}
