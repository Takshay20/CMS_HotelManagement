using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using CMS_HotelBooking.Services.Interfaces;

namespace CMS_HotelBooking.Services.Implementations
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _repository;

        public FeedbackService(IFeedbackRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Feedback>> GetAllAsync() => await _repository.GetAllAsync();

        public async Task<List<Feedback>> GetApprovedAsync(int top = 12) => await _repository.GetApprovedAsync(top);

        public async Task<Feedback?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task<int> SubmitAsync(Feedback model) => await _repository.SubmitAsync(model);

        public async Task<int> ToggleApprovalAsync(int id) => await _repository.ToggleApprovalAsync(id);

        public async Task<int> DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }
}
