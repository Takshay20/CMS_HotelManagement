using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using Dapper;
using System.Data;

namespace CMS_HotelBooking.Repository.Implementations
{
    public class RestoreRecordsRepository : BaseRepository, IRestoreRecordsRepository
    {
        public RestoreRecordsRepository(IConfiguration configuration)
            : base(configuration)
        {
        }

        // Get records
        public async Task<IEnumerable<RestoreRecord>> GetRecordsAsync(string module, string filter)
        {
            using var connection = GetConnection();

            return await connection.QueryAsync<RestoreRecord>(
                "sp_GetRestoreRecords",
                new { Module = module, Filter = filter },
                commandType: CommandType.StoredProcedure
            );
        }

        // Restore deleted restore record
        public async Task<int> RestoreAsync(string module, int id)
        {
            using var connection = GetConnection();

            return await connection.ExecuteScalarAsync<int>(
                "sp_RestoreRecord",
                new { Module = module, Id = id },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
