using System;
using System.Collections.Generic;

namespace CMS_HotelBooking.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public int RoomCategoryId { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal PricePerNight { get; set; }
        public int MaxGuests { get; set; } = 2;
        public string? SizeSqft { get; set; }
        public string? ImagePath { get; set; }
        public string? AmenityIds { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsAvailable { get; set; } = true;
        public int DisplayOrder { get; set; }
        public DateTime CreatedDate { get; set; }

        public string? CategoryName { get; set; }

        public List<RoomImage> Images { get; set; } = new();
        public List<Amenity> AmenityList { get; set; } = new();
    }
}
