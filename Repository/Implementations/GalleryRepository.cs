using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using Dapper;
using System.Data;

namespace CMS_HotelBooking.Repository.Implementations
{
    public class GalleryRepository : BaseRepository, IGalleryRepository
    {
        public GalleryRepository(IConfiguration configuration) : base(configuration) { }

        // Get all gallery records
        public async Task<List<Gallery>> GetAllAsync()
        {
            using var connection = GetConnection();
            var result = await connection.QueryAsync<Gallery>("sp_GetAllGallery", commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        // Get gallery record by id
        public async Task<Gallery?> GetByIdAsync(int id)
        {
            using var connection = GetConnection();
            var parameter = new DynamicParameters();
            parameter.Add("@GalleryId", id);
            return await connection.QueryFirstOrDefaultAsync<Gallery>("sp_GetGalleryById", parameter, commandType: CommandType.StoredProcedure);
        }

        // Save gallery record
        public async Task<int> SaveAsync(Gallery model)
        {
            using var connection = GetConnection();

            var parameter = new DynamicParameters();

            parameter.Add("@GalleryId", model.GalleryId);
            parameter.Add("@Title", model.Title);
            parameter.Add("@ImagePath", model.ImagePath);
            parameter.Add("@Category", model.Category);
            parameter.Add("@DisplayOrder", model.DisplayOrder);
            parameter.Add("@IsActive", model.IsActive);

            return await connection.QuerySingleAsync<int>(
                "sp_SaveGallery",
                parameter,
                commandType: CommandType.StoredProcedure
            );
        }

        // Delete gallery record
        public async Task<int> DeleteAsync(int id)
        {
            using var connection = GetConnection();
            var parameter = new DynamicParameters();
            parameter.Add("@GalleryId", id);
            return await connection.QuerySingleAsync<int>("sp_DeleteGallery", parameter, commandType: CommandType.StoredProcedure);
        }
    }
}
