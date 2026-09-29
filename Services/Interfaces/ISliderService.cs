using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Services.Interfaces
{
    public interface ISliderService
    {
        Task<List<Slider>> GetAllAsync(string? pageKey = null);

        Task<Slider?> GetByIdAsync(int id);

        Task<int> SaveAsync(Slider model);

        Task<int> DeleteAsync(int id);
    }
}