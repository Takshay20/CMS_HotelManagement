using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Repository.Interfaces
{
    public interface IFeedbackRepository
    {
        Task<List<Feedback>> GetAllAsync();
        Task<List<Feedback>> GetApprovedAsync(int top = 12);
        Task<Feedback?> GetByIdAsync(int id);
        Task<int> SubmitAsync(Feedback model);
        Task<int> ToggleApprovalAsync(int id);
        Task<int> DeleteAsync(int id);
    }
}
