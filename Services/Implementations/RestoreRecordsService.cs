using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using CMS_HotelBooking.Services.Interfaces;

namespace CMS_HotelBooking.Services.Implementations
{
    public class RestoreRecordsService : IRestoreRecordsService
    {
        private readonly IRestoreRecordsRepository _repository;

        public RestoreRecordsService(IRestoreRecordsRepository repository)
        {
            _repository = repository;
        }

        // Get records
        public async Task<IEnumerable<RestoreRecord>> GetRecordsAsync(string module, string filter) => await _repository.GetRecordsAsync(module, filter);

        // Restore deleted restore record
        public async Task<int> RestoreAsync(string module, int id) => await _repository.RestoreAsync(module, id);
    }
}
