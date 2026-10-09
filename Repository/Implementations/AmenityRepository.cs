using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using Dapper;
using System.Data;

namespace CMS_HotelBooking.Repository.Implementations
{
    public class AmenityRepository : BaseRepository, IAmenityRepository
    {
        public AmenityRepository(IConfiguration configuration)
            : base(configuration)
        {
        }

        // Get all amenity records
        public async Task<List<Amenity>> GetAllAsync()
        {
            using var connection = GetConnection();

            var result = await connection.QueryAsync<Amenity>( "sp_GetAllAmenity",commandType: CommandType.StoredProcedure);

            return result.ToList();
        }

        // Get amenity record by id
        public async Task<Amenity?> GetByIdAsync(int id)
        {
            using var connection = GetConnection();

            var parameter = new DynamicParameters();

            parameter.Add("@AmenityId",id,DbType.Int32);

            return await connection.QueryFirstOrDefaultAsync<Amenity>("sp_GetAmenityById",parameter,commandType: CommandType.StoredProcedure);
        }

        // Save amenity record
        public async Task<int> SaveAsync(Amenity model)
        {
            using var connection = GetConnection();

            var parameter = new DynamicParameters();

            parameter.Add("@AmenityId",model.AmenityId,DbType.Int32);
            parameter.Add("@Name",model.Name,DbType.String);
            parameter.Add("@IconClass", model.IconClass,DbType.String);
            parameter.Add("@IsActive",model.IsActive,DbType.Boolean);

            return await connection.QuerySingleAsync<int>("sp_SaveAmenity",parameter,commandType: CommandType.StoredProcedure);
        }

        // Delete amenity record
        public async Task<int> DeleteAsync(int id)
        {
            using var connection = GetConnection();

            var parameter = new DynamicParameters();

            parameter.Add("@AmenityId",id,DbType.Int32);

            return await connection.QuerySingleAsync<int>("sp_DeleteAmenity",parameter,commandType: CommandType.StoredProcedure);
        }
    }
}