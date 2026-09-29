using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Repository.Interfaces
{
    public interface IAmenityRepository
    {
        Task<List<Amenity>> GetAllAsync();
        Task<Amenity?> GetByIdAsync(int id);
        Task<int> SaveAsync(Amenity model);
        Task<int> DeleteAsync(int id);
    }
}
