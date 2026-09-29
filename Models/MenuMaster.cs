namespace CMS_HotelBooking.Models
{
    public class MenuMaster
    {
        public int MenuMasterId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
