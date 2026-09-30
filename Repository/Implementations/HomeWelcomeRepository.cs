using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using Dapper;
using System.Data;

namespace CMS_HotelBooking.Repository.Implementations
{
    public class HomeWelcomeRepository : BaseRepository, IHomeWelcomeRepository
    {
        public HomeWelcomeRepository(IConfiguration configuration)
            : base(configuration)
        {
        }

        // Get home welcome record
        public async Task<HomeWelcome?> GetAsync()
        {
            using var connection = GetConnection();

            return await connection.QueryFirstOrDefaultAsync<HomeWelcome>(
                "sp_GetAllHomeWelcome",
                commandType: CommandType.StoredProcedure
            );
        }

        // Update home welcome record
        public async Task<int> UpdateAsync(HomeWelcome model)
        {
            using var connection = GetConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@HomeWelcomeId", model.HomeWelcomeId);
            parameters.Add("@Heading", model.Heading);
            parameters.Add("@SubHeading", model.SubHeading);
            parameters.Add("@Description", model.Description);
            parameters.Add("@Image1Path", model.Image1Path);
            parameters.Add("@Image2Path", model.Image2Path);
            parameters.Add("@Image3Path", model.Image3Path);
            parameters.Add("@ButtonText", model.ButtonText);
            parameters.Add("@ButtonUrl", model.ButtonUrl);
            parameters.Add("@ExperienceYears", model.ExperienceYears);
            parameters.Add("@DisplayOrder", model.DisplayOrder);

            var result = await connection.QuerySingleAsync<int>(
                "sp_SaveHomeWelcome",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
    }
}