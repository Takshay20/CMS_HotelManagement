using Microsoft.Data.SqlClient;
using System.Data;

namespace CMS_HotelBooking.Repository
{
    public abstract class BaseRepository
    {
        private readonly IConfiguration _configuration;

        protected BaseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Get connection
        protected IDbConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
