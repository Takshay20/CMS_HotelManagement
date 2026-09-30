using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using Dapper;
using System.Data;

namespace CMS_HotelBooking.Repository.Implementations
{
    public class HomeWhyChooseUsRepository : BaseRepository, IHomeWhyChooseUsRepository
    {
        public HomeWhyChooseUsRepository(IConfiguration configuration)
            : base(configuration)
        {
        }

        // Get all home why choose us records
        public async Task<List<HomeWhyChooseUs>> GetAllAsync()
        {
            using var connection = GetConnection();

            var result = await connection.QueryAsync<HomeWhyChooseUs>(
                "sp_GetHomeWhyChooseUs",
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }

        // Get home why choose us record by id
        public async Task<HomeWhyChooseUs?> GetByIdAsync(int id)
        {
            using var connection = GetConnection();

            var parameter = new DynamicParameters();

            parameter.Add("@HomeWhyChooseUsId", id);

            return await connection.QueryFirstOrDefaultAsync<HomeWhyChooseUs>(
                "sp_GetHomeWhyChooseUsById",
                parameter,
                commandType: CommandType.StoredProcedure
            );
        }

        // Save home why choose us record
        public async Task<int> SaveAsync(HomeWhyChooseUs model)
        {
            using var connection = GetConnection();

            var parameter = new DynamicParameters();

            parameter.Add("@HomeWhyChooseUsId", model.HomeWhyChooseUsId);
            parameter.Add("@IconClass", model.IconClass);
            parameter.Add("@Title", model.Title);
            parameter.Add("@Description", model.Description);
            parameter.Add("@DisplayOrder", model.DisplayOrder);
            parameter.Add("@IsActive", model.IsActive);

            return await connection.QuerySingleAsync<int>(
                "sp_SaveHomeWhyChooseUs",
                parameter,
                commandType: CommandType.StoredProcedure
            );
        }

        // Delete home why choose us record
        public async Task<int> DeleteAsync(int id)
        {
            using var connection = GetConnection();

            var parameter = new DynamicParameters();

            parameter.Add("@HomeWhyChooseUsId", id);

            return await connection.QuerySingleAsync<int>(
                "sp_DeleteHomeWhyChooseUs",
                parameter,
                commandType: CommandType.StoredProcedure
            );
        }

        // Get all records
        public async Task<IEnumerable<HomeWhyChooseUs>> GetAllRecordsAsync(
    string filter)
        {
            using var connection = GetConnection();

            var parameters = new DynamicParameters();

            parameters.Add(
                "@Filter",
                filter,
                DbType.String
            );

            return await connection.QueryAsync<HomeWhyChooseUs>(
                "sp_GetAllHomeWhyChooseUs",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        // Restore deleted home why choose us record
        public async Task<int> RestoreAsync(int id)
        {
            using var connection = GetConnection();

            var parameters = new DynamicParameters();

            parameters.Add(
                "@HomeWhyChooseUsId",
                id,
                DbType.Int32
            );

            return await connection.ExecuteScalarAsync<int>(
                "sp_RestoreHomeWhyChooseUs",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        // Get deleted date
        public async Task<DateTime?> GetDeletedDateAsync(int id)
        {
            using var connection = GetConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@HomeWhyChooseUsId",id,DbType.Int32);

            return await connection.QueryFirstOrDefaultAsync<DateTime?>("sp_GetHomeWhyChooseUsDeletedDate",parameters,commandType: CommandType.StoredProcedure);
        }
    }
}