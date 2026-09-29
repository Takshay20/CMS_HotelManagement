using CMS_HotelBooking.Models;
namespace CMS_HotelBooking.Repository.Interfaces
{
    public interface IAboutCounterRepository
    {
        Task<List<AboutCounter>> GetAllAsync();
        Task<List<AboutCounter>> GetAllRecordsAsync(string filter);
        Task<AboutCounter?> GetByIdAsync(int id);
        Task<int> SaveAsync(AboutCounter model);
        Task<int> DeleteAsync(int id);
        Task<int> RestoreAsync(int id);
        Task<DateTime?> GetDeletedDateAsync(int id);
    }
}