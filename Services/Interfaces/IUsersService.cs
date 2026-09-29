using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Services.Interfaces
{
    public interface IUsersService
    {
        Task<Users?> GetByEmailAsync(string email);

        Task<Users?> GetByIdAsync(int id);

        Task<List<Users>> GetAllAsync();

        Task<(bool Success, string Message)> RegisterAsync(
            Users model,
            string password,
            string role);

        Task<(bool Success, Users? User, string Message)> LoginAsync(
            string email,
            string password);

        Task<int> ToggleActiveAsync(int id);

        Task<int> DeleteAsync(int id);

        Task<int> UpdatePasswordAsync(
            int userId,
            string passwordHash);

        Task<(bool Success, string Message)> SendResetCodeAsync(
            string email);

        Task<(bool Success, string Message)> VerifyResetCodeAsync(
            string email,
            string code);

        Task<(bool Success, string Message)> ResetPasswordAsync(
            string email,
            string code,
            string newPassword);
    }
}