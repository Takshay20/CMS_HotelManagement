using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Services.Interfaces
{
    public interface IHomeWhyChooseUsService
    {
        Task<List<HomeWhyChooseUs>> GetAllAsync();
        Task<IEnumerable<HomeWhyChooseUs>> GetAllRecordsAsync(string filter);
        Task<HomeWhyChooseUs?> GetByIdAsync(int id);
        Task<int> SaveAsync(HomeWhyChooseUs model);
        Task<int> DeleteAsync(int id);
        Task<int> RestoreAsync(int id);
        
    }
}
