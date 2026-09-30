using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using Dapper;
using System.Data;

namespace CMS_HotelBooking.Repository.Implementations
{
    public class RoomRepository : BaseRepository, IRoomRepository
    {
        public RoomRepository(IConfiguration configuration)
            : base(configuration)
        {
        }

        // Get all room records
        public async Task<List<Room>> GetAllAsync()
        {
            using var connection = GetConnection();

            var result = await connection.QueryAsync<Room>(
                "sp_GetAllRoom",
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }

        // Get room record by id
        public async Task<Room?> GetByIdAsync(int id)
        {
            using var connection = GetConnection();

            var parameter = new DynamicParameters();

            parameter.Add(
                "@RoomId",
                id,
                DbType.Int32
            );

            using var multi =
                await connection.QueryMultipleAsync(
                    "sp_GetRoomById",
                    parameter,
                    commandType: CommandType.StoredProcedure
                );

            var room =
                await multi.ReadFirstOrDefaultAsync<Room>();

            if (room != null)
            {
                var images =
                    await multi.ReadAsync<RoomImage>();

                room.Images =
                    images.ToList();
            }

            return room;
        }

        // Get featured
        public async Task<List<Room>> GetFeaturedAsync(
            int top = 6)
        {
            using var connection = GetConnection();

            var parameter =
                new DynamicParameters();

            parameter.Add(
                "@Top",
                top,
                DbType.Int32
            );

            var result =
                await connection.QueryAsync<Room>(
                    "sp_GetFeaturedRooms",
                    parameter,
                    commandType: CommandType.StoredProcedure
                );

            return result.ToList();
        }

        // Save room record
        public async Task<int> SaveAsync(Room model)
        {
            using var connection = GetConnection();

            var parameter =
                new DynamicParameters();

            parameter.Add(
                "@RoomId",
                model.RoomId,
                DbType.Int32
            );

            parameter.Add(
                "@RoomCategoryId",
                model.RoomCategoryId,
                DbType.Int32
            );

            parameter.Add(
                "@RoomNumber",
                model.RoomNumber,
                DbType.String
            );

            parameter.Add(
                "@Title",
                model.Title,
                DbType.String
            );

            parameter.Add(
                "@Description",
                model.Description,
                DbType.String
            );

            parameter.Add(
                "@PricePerNight",
                model.PricePerNight,
                DbType.Decimal
            );

            parameter.Add(
                "@MaxGuests",
                model.MaxGuests,
                DbType.Int32
            );

            parameter.Add(
                "@SizeSqft",
                model.SizeSqft,
                DbType.Int32
            );

            parameter.Add(
                "@ImagePath",
                model.ImagePath,
                DbType.String
            );

            parameter.Add(
                "@AmenityIds",
                model.AmenityIds,
                DbType.String
            );

            parameter.Add(
                "@IsFeatured",
                model.IsFeatured,
                DbType.Boolean
            );

            parameter.Add(
                "@IsAvailable",
                model.IsAvailable,
                DbType.Boolean
            );

            parameter.Add(
                "@DisplayOrder",
                model.DisplayOrder,
                DbType.Int32
            );

            return await connection.QuerySingleAsync<int>(
                "sp_SaveRoom",
                parameter,
                commandType: CommandType.StoredProcedure
            );
        }

        // Delete room record
        public async Task<int> DeleteAsync(int id)
        {
            using var connection = GetConnection();

            var parameter =
                new DynamicParameters();

            parameter.Add(
                "@RoomId",
                id,
                DbType.Int32
            );

            return await connection.QuerySingleAsync<int>(
                "sp_DeleteRoom",
                parameter,
                commandType: CommandType.StoredProcedure
            );
        }

        // Add image
        public async Task<int> AddImageAsync(
            int roomId,
            string imagePath,
            int displayOrder)
        {
            using var connection = GetConnection();

            var parameter =
                new DynamicParameters();

            parameter.Add(
                "@RoomId",
                roomId,
                DbType.Int32
            );

            parameter.Add(
                "@ImagePath",
                imagePath,
                DbType.String
            );

            parameter.Add(
                "@DisplayOrder",
                displayOrder,
                DbType.Int32
            );

            return await connection.QuerySingleAsync<int>(
                "sp_AddRoomImage",
                parameter,
                commandType: CommandType.StoredProcedure
            );
        }

        // Delete image
        public async Task<int> DeleteImageAsync(
            int roomImageId)
        {
            using var connection = GetConnection();

            var parameter =
                new DynamicParameters();

            parameter.Add(
                "@RoomImageId",
                roomImageId,
                DbType.Int32
            );

            return await connection.QuerySingleAsync<int>(
                "sp_DeleteRoomImage",
                parameter,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}