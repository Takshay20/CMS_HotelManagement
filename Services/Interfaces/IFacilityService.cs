using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Services.Interfaces
{
    public interface IFacilityService
    {
        Task<List<Facility>> GetAllAsync();
        Task<Facility?> GetByIdAsync(int id);
        Task<int> SaveAsync(Facility model);
        Task<int> DeleteAsync(int id);
    }
}
