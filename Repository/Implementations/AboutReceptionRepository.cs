using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using Dapper;
using System.Data;

namespace CMS_HotelBooking.Repository.Implementations
{
    public class AboutReceptionRepository : BaseRepository, IAboutReceptionRepository
    {
        public AboutReceptionRepository(IConfiguration configuration) : base(configuration) { }

        // Get all about reception records
        public async Task<List<AboutReception>> GetAllAsync()
        {
            using var connection = GetConnection();
            var result = await connection.QueryAsync<AboutReception>(
                "sp_GetAboutReception",
                commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        // Get about reception record by id
        public async Task<AboutReception?> GetByIdAsync(int id)
        {
            using var connection = GetConnection();
            var result = await connection.QueryAsync<AboutReception>(
                "sp_GetAboutReception",
                commandType: CommandType.StoredProcedure);
            return result.FirstOrDefault(x => x.AboutReceptionId == id);
        }

        // Save about reception record
        public async Task<int> SaveAsync(AboutReception model)
        {
            using var connection = GetConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@AboutReceptionId", model.AboutReceptionId);
            parameters.Add("@Title", model.Title);
            parameters.Add("@Description", model.Description);
            parameters.Add("@ImagePath", model.ImagePath);

            return await connection.QuerySingleAsync<int>(
                "sp_SaveAboutReception",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        // Delete about reception record
        public Task<int> DeleteAsync(int id)
        {
            throw new NotSupportedException(
                "SQL does not contain a delete procedure for AboutReception. Use the SQL-supported Save/Get operations.");
        }
    }
}
