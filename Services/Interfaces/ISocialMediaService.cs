using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Services.Interfaces
{
    public interface ISocialMediaService
    {
        Task<List<SocialMedia>> GetAllAsync();
        Task<List<SocialMedia>> GetActiveAsync();
        Task<SocialMedia?> GetByIdAsync(int id);
        Task<int> SaveAsync(SocialMedia model);
        Task<int> DeleteAsync(int id);
    }
}
