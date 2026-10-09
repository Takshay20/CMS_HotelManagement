using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using Dapper;
using System.Data;

namespace CMS_HotelBooking.Repository.Implementations
{
    public class AboutCounterRepository : BaseRepository, IAboutCounterRepository
    {
        public AboutCounterRepository(IConfiguration configuration) : base(configuration)
        {
        }

        // Get all about counter records
        public async Task<List<AboutCounter>> GetAllAsync()
        {
            using var connection = GetConnection();

            var result = await connection.QueryAsync<AboutCounter>("sp_GetAllAboutCounter",commandType: CommandType.StoredProcedure);

            return result.ToList();
        }

        // Get all records
        public async Task<List<AboutCounter>> GetAllRecordsAsync(string filter)
        {
            using var connection = GetConnection();

            var parameter = new DynamicParameters();
            parameter.Add("@Filter", filter);

            var result = await connection.QueryAsync<AboutCounter>("sp_GetAllAboutCounterRecords",parameter,commandType: CommandType.StoredProcedure);

            return result.ToList();
        }

        // Get about counter record by id
        public async Task<AboutCounter?> GetByIdAsync(int id)
        {
            using var connection = GetConnection();

            var parameter = new DynamicParameters();
            parameter.Add("@AboutCounterId", id);

            return await connection.QueryFirstOrDefaultAsync<AboutCounter>("sp_GetAboutCounterById",parameter,commandType: CommandType.StoredProcedure);
        }

        // Save about counter record
        public async Task<int> SaveAsync(AboutCounter model)
        {
            using var connection = GetConnection();

            var parameter = new DynamicParameters();

            parameter.Add("@AboutCounterId", model.AboutCounterId);
            parameter.Add("@IconClass", model.IconClass);
            parameter.Add("@Number", model.Number);
            parameter.Add("@Suffix", model.Suffix);
            parameter.Add("@Label", model.Label);
            parameter.Add("@DisplayOrder", model.DisplayOrder);
            parameter.Add("@IsActive", model.IsActive);

            return await connection.QueryFirstOrDefaultAsync<int>("sp_SaveAboutCounter",parameter,commandType: CommandType.StoredProcedure);
        }

        // Delete about counter record
        public async Task<int> DeleteAsync(int id)
        {
            using var connection = GetConnection();

            var parameter = new DynamicParameters();
            parameter.Add("@AboutCounterId", id);

            return await connection.QueryFirstOrDefaultAsync<int>("sp_DeleteAboutCounter",parameter,commandType: CommandType.StoredProcedure);
        }

        // Restore deleted about counter record
        public async Task<int> RestoreAsync(int id)
        {
            using var connection = GetConnection();

            var parameter = new DynamicParameters();
            parameter.Add("@AboutCounterId", id);

            return await connection.QueryFirstOrDefaultAsync<int>("sp_RestoreAboutCounter",parameter,commandType: CommandType.StoredProcedure);
        }

     
    }
}