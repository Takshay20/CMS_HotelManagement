using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using CMS_HotelBooking.Services.Interfaces;

namespace CMS_HotelBooking.Services.Implementations
{
    public class SiteSettingService : ISiteSettingService
    {
        private readonly ISiteSettingRepository _repository;

        public SiteSettingService(ISiteSettingRepository repository)
        {
            _repository = repository;
        }

        public async Task<SiteSetting?> GetAsync() => await _repository.GetAsync();

        public async Task<int> SaveAsync(SiteSetting model) => await _repository.SaveAsync(model);
    }
}
