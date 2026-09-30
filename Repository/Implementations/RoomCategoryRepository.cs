using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using Dapper;
using System.Data;

namespace CMS_HotelBooking.Repository.Implementations
{
    public class RoomCategoryRepository : BaseRepository, IRoomCategoryRepository
    {
        public RoomCategoryRepository(IConfiguration configuration)
            : base(configuration)
        {
        }

        // Get all room category records
        public async Task<List<RoomCategory>> GetAllAsync()
        {
            using var connection = GetConnection();

            var result = await connection.QueryAsync<RoomCategory>(
                "sp_GetAllRoomCategory",
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }

        // Get room category record by id
        public async Task<RoomCategory?> GetByIdAsync(int id)
        {
            using var connection = GetConnection();

            var parameters = new DynamicParameters();

            parameters.Add(
                "@RoomCategoryId",
                id,
                DbType.Int32
            );

            return await connection.QueryFirstOrDefaultAsync<RoomCategory>(
                "sp_GetRoomCategoryById",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        // Save room category record
        public async Task<int> SaveAsync(RoomCategory model)
        {
            using var connection = GetConnection();

            var parameters = new DynamicParameters();

            parameters.Add(
                "@RoomCategoryId",
                model.RoomCategoryId,
                DbType.Int32
            );

            parameters.Add(
                "@Name",
                model.Name,
                DbType.String
            );

            parameters.Add(
                "@Description",
                model.Description,
                DbType.String
            );

            parameters.Add(
                "@DisplayOrder",
                model.DisplayOrder,
                DbType.Int32
            );

            parameters.Add(
                "@IsActive",
                model.IsActive,
                DbType.Boolean
            );

            return await connection.QuerySingleAsync<int>(
                "sp_SaveRoomCategory",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        // Delete room category record
        public async Task<int> DeleteAsync(int id)
        {
            using var connection = GetConnection();

            var parameters = new DynamicParameters();

            parameters.Add(
                "@RoomCategoryId",
                id,
                DbType.Int32
            );

            return await connection.QuerySingleAsync<int>(
                "sp_DeleteRoomCategory",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}