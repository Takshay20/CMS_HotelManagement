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

        public async Task<List<Facility>> GetAllAsync() => await _repository.GetAllAsync();

        public async Task<Facility?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task<int> SaveAsync(Facility model) => await _repository.SaveAsync(model);

        public async Task<int> DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }
}
