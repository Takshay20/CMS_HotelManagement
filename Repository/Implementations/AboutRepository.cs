using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using Dapper;
using System.Data;

namespace CMS_HotelBooking.Repository.Implementations
{
    public class AboutRepository : BaseRepository, IAboutRepository
    {
        public AboutRepository(IConfiguration configuration)
            : base(configuration)
        {
        }

        // Get about record
        public async Task<About?> GetAsync()
        {
            using var connection = GetConnection();

            return await connection.QueryFirstOrDefaultAsync<About>(
                "sp_GetAbout",
                commandType: CommandType.StoredProcedure
            );
        }

        // Save about record
        public async Task<int> SaveAsync(About model)
        {
            using var connection = GetConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@AboutId", model.AboutId);
            parameters.Add("@Title", model.Title);
            parameters.Add("@SubTitle", model.SubTitle);
            parameters.Add("@ShortDescription", model.ShortDescription);
            parameters.Add("@FullDescription", model.FullDescription);
            parameters.Add("@Image", model.Image);
            parameters.Add("@YearsOfExperience", model.YearsOfExperience);
            parameters.Add("@TotalRooms", model.TotalRooms);
            parameters.Add("@HappyGuests", model.HappyGuests);

            var result = await connection.QuerySingleAsync<int>(
                "sp_SaveAbout",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
    }
}