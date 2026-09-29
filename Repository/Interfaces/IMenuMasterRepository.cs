using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Repository.Interfaces
{
    public interface IMenuMasterRepository
    {
        Task<List<MenuMaster>> GetAllAsync();
        Task<List<MenuMaster>> GetActiveAsync();
        Task<MenuMaster?> GetByIdAsync(int id);
        Task<int> SaveAsync(MenuMaster model);
        Task<int> DeleteAsync(int id);
    }
}
