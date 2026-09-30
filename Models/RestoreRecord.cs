namespace CMS_HotelBooking.Models
{
    public class RestoreRecord
    {
        public int Id { get; set; }
        public string? IconClass { get; set; }
        public string? Title { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Number { get; set; }
        public string? Suffix { get; set; }
        public string? Label { get; set; }
        public string? RoomNumber { get; set; }
        public decimal? PricePerNight { get; set; }
        public int? MaxGuests { get; set; }
        public string? ImagePath { get; set; }
        public string? Category { get; set; }
        public string? PageKey { get; set; }
        public string? Url { get; set; }
        public string? PlatformName { get; set; }
        public int? DisplayOrder { get; set; }
        public bool IsDeleted { get; set; }
        public int? RemainingSeconds { get; set; }
    }
}
