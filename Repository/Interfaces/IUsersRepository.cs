using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Repository.Interfaces
{
    public interface IUsersRepository
    {

        Task<Users?> GetByEmailAsync(string email);
        Task<Users?> GetByIdAsync(int id);
        Task<List<Users>> GetAllAsync();
        Task<int> RegisterAsync(Users model,string role);
        Task<int> ToggleActiveAsync(int id);
        Task<int> DeleteAsync(int id);
        Task<int> SaveResetCodeAsync(int userId,string resetCode,DateTime expiry);
        Task<bool> VerifyResetCodeAsync(int userId,string resetCode);
        Task<int> ClearResetCodeAsync(int userId);
        Task<int> UpdatePasswordAsync(int userId,string passwordHash);
    }
}