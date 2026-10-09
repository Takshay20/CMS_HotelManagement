using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Services.Interfaces
{
    public interface IAboutStoryService
    {
        Task<List<AboutStory>> GetAllAsync();
        Task<AboutStory?> GetByIdAsync(int id);
        Task<int> SaveAsync(AboutStory model);


    }
}
