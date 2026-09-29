namespace CMS_HotelBooking.Models
{
    public class SiteSetting
    {
        public int SiteSettingId { get; set; }
        public string SiteName { get; set; } = "Royal Paradise Hotel";
        public string? Tagline { get; set; }
        public string? LogoPath { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }
}
