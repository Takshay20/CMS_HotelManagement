using System.Collections.Generic;

namespace CMS_HotelBooking.Models
{
    public class ArrivalsDeparturesModel
    {
        public List<Booking> Arrivals { get; set; } = new();
        public List<Booking> Departures { get; set; } = new();
    }
}
