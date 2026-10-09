using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Services.Interfaces
{
    public interface IAboutCtaService
    {
        Task<List<AboutCta>> GetAllAsync();
        Task<AboutCta?> GetByIdAsync(int id);
        Task<int> SaveAsync(AboutCta model);
    }
}
