using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using Dapper;
using System.Data;

namespace CMS_HotelBooking.Repository.Implementations
{
    public class SiteSettingRepository : BaseRepository, ISiteSettingRepository
    {
        public SiteSettingRepository(IConfiguration configuration) : base(configuration) { }

        // Get site setting record
        public async Task<SiteSetting?> GetAsync()
        {
            using var connection = GetConnection();
            return await connection.QueryFirstOrDefaultAsync<SiteSetting>("sp_GetSiteSetting", commandType: CommandType.StoredProcedure);
        }

        // Save site setting record
        public async Task<int> SaveAsync(SiteSetting model)
        {
            using var connection = GetConnection();
            var parameter = new DynamicParameters();
            parameter.Add("@SiteSettingId", model.SiteSettingId);
            parameter.Add("@SiteName", model.SiteName);
            parameter.Add("@Tagline", model.Tagline);
            parameter.Add("@LogoPath", model.LogoPath);
            parameter.Add("@Phone", model.Phone);
            parameter.Add("@Email", model.Email);
            parameter.Add("@Address", model.Address);
            return await connection.QueryFirstOrDefaultAsync<int>("sp_SaveSiteSetting", parameter, commandType: CommandType.StoredProcedure);
        }
    }
}
