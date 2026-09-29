using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using CMS_HotelBooking.Services.Interfaces;

namespace CMS_HotelBooking.Services.Implementations
{
    public class AboutReceptionService : IAboutReceptionService
    {
        private readonly IAboutReceptionRepository _repository;

        public AboutReceptionService(IAboutReceptionRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<AboutReception>> GetAllAsync() => await _repository.GetAllAsync();

        public async Task<AboutReception?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task<int> SaveAsync(AboutReception model) => await _repository.SaveAsync(model);

        public async Task<int> DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }
}
