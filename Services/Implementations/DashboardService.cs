using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using CMS_HotelBooking.Services.Interfaces;

namespace CMS_HotelBooking.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repository;

        public DashboardService(IDashboardRepository repository)
        {
            _repository = repository;
        }

        public async Task<DashboardCounts> GetCountsAsync() => await _repository.GetCountsAsync();

        public async Task<ArrivalsDeparturesModel> GetUpcomingArrivalsDeparturesAsync() => await _repository.GetUpcomingArrivalsDeparturesAsync();
    }
}
