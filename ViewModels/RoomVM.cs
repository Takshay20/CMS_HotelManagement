using CMS_HotelBooking.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace CMS_HotelBooking.ViewModels
{
    public class RoomVM
    {
        public Room Room { get; set; } = new Room();
        public IFormFile? ImageFile { get; set; }
        public List<IFormFile>? GalleryFiles { get; set; }
    }
}
