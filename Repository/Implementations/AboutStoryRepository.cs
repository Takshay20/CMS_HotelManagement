using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using Dapper;
using System.Data;

namespace CMS_HotelBooking.Repository.Implementations
{
    public class AboutStoryRepository : BaseRepository, IAboutStoryRepository
    {
        public AboutStoryRepository(IConfiguration configuration)
            : base(configuration)
        {
        }

        // Get all about story records
        public async Task<List<AboutStory>> GetAllAsync() 
        {
            using var connection = GetConnection();

            var result = await connection.QueryAsync<AboutStory>("sp_GetAboutStory", commandType: CommandType.StoredProcedure );

            return result.ToList();
        }

        // Get about story record by id
        public async Task<AboutStory?> GetByIdAsync(int id)
        {
            using var connection = GetConnection();

            var result = await connection.QueryAsync<AboutStory>("sp_GetAboutStory", commandType: CommandType.StoredProcedure);

            return result.FirstOrDefault( x => x.AboutStoryId == id);
        }

        // Save about story record
        public async Task<int> SaveAsync(AboutStory model)
        {
            using var connection = GetConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@AboutStoryId", model.AboutStoryId);

            parameters.Add("@Heading",model.Heading);

            parameters.Add("@SubHeading", model.SubHeading);

            parameters.Add( "@Description", model.Description);

            parameters.Add("@ImagePath", model.ImagePath);

            var result = await connection.QuerySingleAsync<int>("sp_SaveAboutStory",parameters,commandType: CommandType.StoredProcedure);

            return result;
        }

    }
}