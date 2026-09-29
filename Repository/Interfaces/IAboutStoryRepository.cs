using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Repository.Interfaces
{
    public interface IAboutStoryRepository
    {
        Task<List<AboutStory>> GetAllAsync();
        Task<AboutStory?> GetByIdAsync(int id);
        Task<int> SaveAsync(AboutStory model);
        Task<int> DeleteAsync(int id);
    }
}
