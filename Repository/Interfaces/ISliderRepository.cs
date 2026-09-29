using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Repository.Interfaces
{
    public interface ISliderRepository
    {
        Task<List<Slider>> GetAllAsync(string? pageKey = null);

        Task<Slider?> GetByIdAsync(int id);

        Task<int> SaveAsync(Slider model);

        Task<int> DeleteAsync(int id);
    }
}