using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using Dapper;
using System.Data;

namespace CMS_HotelBooking.Repository.Implementations
{
    public class FooterRepository : BaseRepository, IFooterRepository
    {
        public FooterRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<Footer?> GetAsync()
        {
            using var connection = GetConnection();
            return await connection.QueryFirstOrDefaultAsync<Footer>("sp_GetFooter", commandType: CommandType.StoredProcedure);
        }

        public async Task<int> SaveAsync(Footer model)
        {
            using var connection = GetConnection();
            var parameter = new DynamicParameters();
            parameter.Add("@FooterId", model.FooterId);
            parameter.Add("@AboutText", model.AboutText);
            parameter.Add("@Phone", model.Phone);
            parameter.Add("@Email", model.Email);
            parameter.Add("@Address", model.Address);
            parameter.Add("@CopyrightText", model.CopyrightText);
            return await connection.QueryFirstOrDefaultAsync<int>("sp_SaveFooter", parameter, commandType: CommandType.StoredProcedure);
        }
    }
}
