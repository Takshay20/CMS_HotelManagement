using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Repository.Interfaces
{
    public interface ISocialMediaRepository
    {
        Task<List<SocialMedia>> GetAllAsync();
        Task<List<SocialMedia>> GetActiveAsync();
        Task<SocialMedia?> GetByIdAsync(int id);
        Task<int> SaveAsync(SocialMedia model);
        Task<int> DeleteAsync(int id);
    }
}
