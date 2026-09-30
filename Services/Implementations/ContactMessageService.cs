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

        // Get all contact message records
        public async Task<List<ContactMessage>> GetAllAsync() => await _repository.GetAllAsync();

        // Get contact message record by id
        public async Task<ContactMessage?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        // Submit contact message form
        public async Task<int> SubmitAsync(ContactMessage model) => await _repository.SubmitAsync(model);

        // Delete contact message record
        public async Task<int> DeleteAsync(int id) => await _repository.DeleteAsync(id);

        // Save admin reply
        public async Task<int> ReplyAsync(int id, string adminReply) => await _repository.ReplyAsync(id, adminReply);

        // Get by email
        public async Task<List<ContactMessage>> GetByEmailAsync(string email) => await _repository.GetByEmailAsync(email);
    }
}
