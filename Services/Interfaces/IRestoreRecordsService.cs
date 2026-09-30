using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Services.Interfaces
{
    public interface IRestoreRecordsService
    {
        Task<IEnumerable<RestoreRecord>> GetRecordsAsync(string module, string filter);
        Task<int> RestoreAsync(string module, int id);
    }
}
