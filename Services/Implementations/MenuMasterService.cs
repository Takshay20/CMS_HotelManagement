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

        public async Task<List<MenuMaster>> GetAllAsync() => await _repository.GetAllAsync();

        public async Task<List<MenuMaster>> GetActiveAsync() => await _repository.GetActiveAsync();

        public async Task<MenuMaster?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task<int> SaveAsync(MenuMaster model) => await _repository.SaveAsync(model);

        public async Task<int> DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }
}
