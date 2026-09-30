using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using Dapper;
using System.Data;

namespace CMS_HotelBooking.Repository.Implementations
{
    public class SliderRepository : BaseRepository, ISliderRepository
    {
        public SliderRepository(IConfiguration configuration)
            : base(configuration)
        {
        }

        // Get all slider records
        public async Task<List<Slider>> GetAllAsync(string? pageKey = null)
        {
            using var connection = GetConnection();

            var parameter = new DynamicParameters();

            parameter.Add("@PageKey", pageKey);

            var result = await connection.QueryAsync<Slider>(
                "sp_GetAllSlider",
                parameter,
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }

        // Get slider record by id
        public async Task<Slider?> GetByIdAsync(int id)
        {
            using var connection = GetConnection();

            var parameter = new DynamicParameters();

            parameter.Add("@SliderId", id);

            return await connection.QueryFirstOrDefaultAsync<Slider>(
                "sp_GetSliderById",
                parameter,
                commandType: CommandType.StoredProcedure
            );
        }

        // Save slider record
        public async Task<int> SaveAsync(Slider model)
        {
            using var connection = GetConnection();

            var parameter = new DynamicParameters();

            parameter.Add("@SliderId", model.SliderId);
            parameter.Add("@PageKey", model.PageKey);
            parameter.Add("@ImagePath", model.ImagePath);
            parameter.Add("@Title", model.Title);
            parameter.Add("@SubTitle", model.SubTitle);
            parameter.Add("@ButtonText", model.ButtonText);
            parameter.Add("@ButtonUrl", model.ButtonUrl);
            parameter.Add("@DisplayOrder", model.DisplayOrder);
            parameter.Add("@IsActive", model.IsActive);

            return await connection.QuerySingleAsync<int>(
                "sp_SaveSlider",
                parameter,
                commandType: CommandType.StoredProcedure
            );
        }

        // Delete slider record
        public async Task<int> DeleteAsync(int id)
        {
            using var connection = GetConnection();

            var parameter = new DynamicParameters();

            parameter.Add("@SliderId", id);

            return await connection.QueryFirstOrDefaultAsync<int>(
                "sp_DeleteSlider",
                parameter,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}