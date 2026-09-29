using Microsoft.Data.SqlClient;
using System.Data;

namespace CMS_HotelBooking.Data
{
    
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }

    public class SqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public SqlConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("DefaultConnection connection string is missing in appsettings.json");
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
