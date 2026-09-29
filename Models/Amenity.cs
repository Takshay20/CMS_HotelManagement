namespace CMS_HotelBooking.Models
{
    public class Amenity
    {
        public int AmenityId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? IconClass { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
