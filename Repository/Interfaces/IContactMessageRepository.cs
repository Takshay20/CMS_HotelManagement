using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Repository.Interfaces
{
    public interface IContactMessageRepository
    {
        Task<List<ContactMessage>> GetAllAsync();
        Task<ContactMessage?> GetByIdAsync(int id);
        Task<int> SubmitAsync(ContactMessage model);
        Task<int> DeleteAsync(int id);
        Task<int> ReplyAsync(int id, string adminReply);
        Task<List<ContactMessage>> GetByEmailAsync(string email);
    }
}
