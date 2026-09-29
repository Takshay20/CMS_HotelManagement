using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using Dapper;
using System.Data;

namespace CMS_HotelBooking.Repository.Implementations
{
    public class AboutCtaRepository : BaseRepository, IAboutCtaRepository
    {
        public AboutCtaRepository(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<List<AboutCta>> GetAllAsync()
        {
            using var connection = GetConnection();

            var result = await connection.QueryAsync<AboutCta>(
                "sp_GetAboutCta",
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }

        public async Task<AboutCta?> GetByIdAsync(int id)
        {
            using var connection = GetConnection();

            var result = await connection.QueryAsync<AboutCta>(
                "sp_GetAboutCta",
                commandType: CommandType.StoredProcedure
            );

            return result.FirstOrDefault(x => x.AboutCtaId == id);
        }

        public async Task<int> SaveAsync(AboutCta model)
        {
            using var connection = GetConnection();

            var parameter = new DynamicParameters();

            parameter.Add("@AboutCtaId", model.AboutCtaId);
            parameter.Add("@Heading", model.Heading);
            parameter.Add("@SubHeading", model.SubHeading);
            parameter.Add("@ButtonText", model.ButtonText);
            parameter.Add("@ButtonUrl", model.ButtonUrl);
            parameter.Add("@ImagePath", model.ImagePath);

            var result = await connection.QuerySingleAsync<int>(
                "sp_SaveAboutCta",
                parameter,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

        public async Task<int> DeleteAsync(int id)
        {
            return 0;
        }
    }
}