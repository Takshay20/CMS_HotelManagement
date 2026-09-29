using System;

namespace CMS_HotelBooking.Models
{
    public class HomeWhyChooseUs
    {
        public int HomeWhyChooseUsId { get; set; }
        public string? IconClass { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
