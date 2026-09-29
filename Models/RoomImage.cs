namespace CMS_HotelBooking.Models
{
    public class RoomImage
    {
        public int RoomImageId { get; set; }
        public int RoomId { get; set; }
        public string ImagePath { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
    }
}
