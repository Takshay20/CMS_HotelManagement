using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using CMS_HotelBooking.Services.Interfaces;

namespace CMS_HotelBooking.Services.Implementations
{
    public class MenuMasterService : IMenuMasterService
    {
        private readonly IMenuMasterRepository _repository;

        public MenuMasterService(IMenuMasterRepository repository)
        {
            _repository = repository;
        }

        // Get all menu master records
        public async Task<List<MenuMaster>> GetAllAsync() => await _repository.GetAllAsync();

        // Get active
        public async Task<List<MenuMaster>> GetActiveAsync() => await _repository.GetActiveAsync();

        // Get menu master record by id
        public async Task<MenuMaster?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        // Save menu master record
        public async Task<int> SaveAsync(MenuMaster model) => await _repository.SaveAsync(model);

        // Delete menu master record
        public async Task<int> DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }
}
