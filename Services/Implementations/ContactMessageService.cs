using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using CMS_HotelBooking.Services.Interfaces;

namespace CMS_HotelBooking.Services.Implementations
{
    public class ContactMessageService : IContactMessageService
    {
        private readonly IContactMessageRepository _repository;

        public ContactMessageService(IContactMessageRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ContactMessage>> GetAllAsync() => await _repository.GetAllAsync();

        public async Task<ContactMessage?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task<int> SubmitAsync(ContactMessage model) => await _repository.SubmitAsync(model);

        public async Task<int> DeleteAsync(int id) => await _repository.DeleteAsync(id);

        public async Task<int> ReplyAsync(int id, string adminReply) => await _repository.ReplyAsync(id, adminReply);

        public async Task<List<ContactMessage>> GetByEmailAsync(string email) => await _repository.GetByEmailAsync(email);
    }
}
