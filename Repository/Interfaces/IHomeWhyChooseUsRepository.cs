using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Repository.Interfaces
{
    public interface IHomeWhyChooseUsRepository
    {
        Task<List<HomeWhyChooseUs>> GetAllAsync();
        Task<IEnumerable<HomeWhyChooseUs>> GetAllRecordsAsync(string filter);
        Task<HomeWhyChooseUs?> GetByIdAsync(int id);
        Task<int> SaveAsync(HomeWhyChooseUs model);
        Task<int> DeleteAsync(int id);
        Task<int> RestoreAsync(int id);
        Task<DateTime?> GetDeletedDateAsync(int id);

    }
}
