using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Repository.Interfaces
{
    public interface IGalleryRepository
    {
        Task<List<Gallery>> GetAllAsync();
        Task<Gallery?> GetByIdAsync(int id);
        Task<int> SaveAsync(Gallery model);
        Task<int> DeleteAsync(int id);
    }
}
