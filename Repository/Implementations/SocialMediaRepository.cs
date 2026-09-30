using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using Dapper;
using System.Data;

namespace CMS_HotelBooking.Repository.Implementations
{
    public class SocialMediaRepository : BaseRepository, ISocialMediaRepository
    {
        public SocialMediaRepository(IConfiguration configuration) : base(configuration) { }

        // Get all social media records
        public async Task<List<SocialMedia>> GetAllAsync()
        {
            using var connection = GetConnection();
            var result = await connection.QueryAsync<SocialMedia>("sp_GetAllSocialMedia", commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        // Get active
        public async Task<List<SocialMedia>> GetActiveAsync()
        {
            using var connection = GetConnection();
            var result = await connection.QueryAsync<SocialMedia>("sp_GetActiveSocialMedia", commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        // Get social media record by id
        public async Task<SocialMedia?> GetByIdAsync(int id)
        {
            using var connection = GetConnection();
            var parameter = new DynamicParameters();
            parameter.Add("@SocialMediaId", id);
            return await connection.QueryFirstOrDefaultAsync<SocialMedia>("sp_GetSocialMediaById", parameter, commandType: CommandType.StoredProcedure);
        }

        // Save social media record
        public async Task<int> SaveAsync(SocialMedia model)
        {
            using var connection = GetConnection();
            var parameter = new DynamicParameters();
            parameter.Add("@SocialMediaId", model.SocialMediaId);
            parameter.Add("@PlatformName", model.PlatformName);
            parameter.Add("@IconClass", model.IconClass);
            parameter.Add("@Url", model.Url);
            parameter.Add("@DisplayOrder", model.DisplayOrder);
            parameter.Add("@IsActive", model.IsActive);
            return await connection.QueryFirstOrDefaultAsync<int>("sp_SaveSocialMedia", parameter, commandType: CommandType.StoredProcedure);
        }

        // Delete social media record
        public async Task<int> DeleteAsync(int id)
        {
            using var connection = GetConnection();
            var parameter = new DynamicParameters();
            parameter.Add("@SocialMediaId", id);
            return await connection.QueryFirstOrDefaultAsync<int>("sp_DeleteSocialMedia", parameter, commandType: CommandType.StoredProcedure);
        }
    }
}
