using CMS_HotelBooking.Models;
namespace CMS_HotelBooking.Services.Interfaces
{
    public interface IAboutCounterService
    {
        Task<List<AboutCounter>> GetAllAsync();
        Task<List<AboutCounter>> GetAllRecordsAsync(string filter);
        Task<AboutCounter?> GetByIdAsync(int id);
        Task<int> SaveAsync(AboutCounter model);
        Task<int> DeleteAsync(int id);
        Task<int> RestoreAsync(int id);
    }
}