using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Repository.Interfaces
{
    public interface IAboutReceptionRepository
    {
        Task<List<AboutReception>> GetAllAsync();
        Task<AboutReception?> GetByIdAsync(int id);
        Task<int> SaveAsync(AboutReception model);
        Task<int> DeleteAsync(int id);
    }
}
