using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Repository.Interfaces
{
    public interface IAboutCtaRepository
    {
        Task<List<AboutCta>> GetAllAsync();
        Task<AboutCta?> GetByIdAsync(int id);
        Task<int> SaveAsync(AboutCta model);
        Task<int> DeleteAsync(int id);
    }
}
