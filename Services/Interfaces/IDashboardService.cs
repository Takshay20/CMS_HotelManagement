using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardCounts> GetCountsAsync();
        Task<ArrivalsDeparturesModel> GetUpcomingArrivalsDeparturesAsync();
    }
}
