using CMS_HotelBooking.Models;

namespace CMS_HotelBooking.Repository.Interfaces
{
    public interface IDashboardRepository
    {
        Task<DashboardCounts> GetCountsAsync();
        Task<ArrivalsDeparturesModel> GetUpcomingArrivalsDeparturesAsync();
    }
}
