using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using Dapper;
using System.Data;

namespace CMS_HotelBooking.Repository.Implementations
{
    public class DashboardRepository : BaseRepository, IDashboardRepository
    {
        public DashboardRepository(IConfiguration configuration) : base(configuration) { }

        // Get counts
        public async Task<DashboardCounts> GetCountsAsync()
        {
            using var connection = GetConnection();
            var result = await connection.QueryFirstOrDefaultAsync<DashboardCounts>("sp_GetDashboardCounts", commandType: CommandType.StoredProcedure);
            return result ?? new DashboardCounts();
        }

        // Get upcoming arrivals departures
        public async Task<ArrivalsDeparturesModel> GetUpcomingArrivalsDeparturesAsync()
        {
            using var connection = GetConnection();
            using var multi = await connection.QueryMultipleAsync("sp_GetUpcomingArrivalsDepartures", commandType: CommandType.StoredProcedure);
            var arrivals = (await multi.ReadAsync<Booking>()).ToList();
            var departures = (await multi.ReadAsync<Booking>()).ToList();
            return new ArrivalsDeparturesModel { Arrivals = arrivals, Departures = departures };
        }
    }
}
