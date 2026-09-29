using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Services.Interfaces
{
    public interface IMenuMasterService
    {
        Task<List<MenuMaster>> GetAllAsync();
        Task<List<MenuMaster>> GetActiveAsync();
        Task<MenuMaster?> GetByIdAsync(int id);
        Task<int> SaveAsync(MenuMaster model);
        Task<int> DeleteAsync(int id);
    }
}
