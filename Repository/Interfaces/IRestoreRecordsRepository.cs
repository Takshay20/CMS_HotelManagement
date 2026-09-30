using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Repository.Interfaces
{
    public interface IRestoreRecordsRepository
    {
        Task<IEnumerable<RestoreRecord>> GetRecordsAsync(string module, string filter);
        Task<int> RestoreAsync(string module, int id);
    }
}
