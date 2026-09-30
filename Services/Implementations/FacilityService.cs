using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using CMS_HotelBooking.Services.Interfaces;

namespace CMS_HotelBooking.Services.Implementations
{
    public class FacilityService : IFacilityService
    {
        private readonly IFacilityRepository _repository;

        public FacilityService(IFacilityRepository repository)
        {
            _repository = repository;
        }

        // Get all facility records
        public async Task<List<Facility>> GetAllAsync() => await _repository.GetAllAsync();

        // Get facility record by id
        public async Task<Facility?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        // Save facility record
        public async Task<int> SaveAsync(Facility model) => await _repository.SaveAsync(model);

        // Delete facility record
        public async Task<int> DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }
}
