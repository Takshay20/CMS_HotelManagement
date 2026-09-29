using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Services.Interfaces
{
    public interface IAboutReceptionService
    {
        Task<List<AboutReception>> GetAllAsync();
        Task<AboutReception?> GetByIdAsync(int id);
        Task<int> SaveAsync(AboutReception model);
        Task<int> DeleteAsync(int id);
    }
}
