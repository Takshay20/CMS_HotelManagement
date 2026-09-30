using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using Dapper;
using System.Data;

namespace CMS_HotelBooking.Repository.Implementations
{
    public class FacilityRepository : BaseRepository, IFacilityRepository
    {
        public FacilityRepository(IConfiguration configuration) : base(configuration) { }

        // Get all facility records
        public async Task<List<Facility>> GetAllAsync()
        {
            using var connection = GetConnection();
            var result = await connection.QueryAsync<Facility>("sp_GetAllFacility", commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        // Get facility record by id
        public async Task<Facility?> GetByIdAsync(int id)
        {
            using var connection = GetConnection();
            var parameter = new DynamicParameters();
            parameter.Add("@FacilityId", id);
            return await connection.QueryFirstOrDefaultAsync<Facility>("sp_GetFacilityById", parameter, commandType: CommandType.StoredProcedure);
        }

        // Save facility record
        public async Task<int> SaveAsync(Facility model)
        {
            using var connection = GetConnection();
            var parameter = new DynamicParameters();
            parameter.Add("@FacilityId", model.FacilityId);
            parameter.Add("@Title", model.Title);
            parameter.Add("@Description", model.Description);
            parameter.Add("@ImagePath", model.ImagePath);
            parameter.Add("@DisplayOrder", model.DisplayOrder);
            parameter.Add("@IsActive", model.IsActive);
            return await connection.QuerySingleAsync<int>("sp_SaveFacility", parameter, commandType: CommandType.StoredProcedure);
        }

        // Delete facility record
        public async Task<int> DeleteAsync(int id)
        {
            using var connection = GetConnection();
            var parameter = new DynamicParameters();
            parameter.Add("@FacilityId", id);
            return await connection.QuerySingleAsync<int>("sp_DeleteFacility", parameter, commandType: CommandType.StoredProcedure);
        }
    }
}
