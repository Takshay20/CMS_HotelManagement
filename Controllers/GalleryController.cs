using CMS_HotelBooking.Models;
using CMS_HotelBooking.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CMS_HotelBooking.Controllers
{
    public class GalleryViewModel
    {
        public List<Slider> Sliders { get; set; } = new();
        public List<Gallery> Images { get; set; } = new();
    }

    public class GalleryController : Controller
    {
        private readonly ISliderService _sliderService;
        private readonly IGalleryService _galleryService;

        public GalleryController(ISliderService sliderService, IGalleryService galleryService)
        {
            _sliderService = sliderService;
            _galleryService = galleryService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new GalleryViewModel
            {
                Sliders = await _sliderService.GetAllAsync("Gallery"),
                Images = (await _galleryService.GetAllAsync()).Where(g => g.IsActive).OrderBy(g => g.DisplayOrder).ToList()
            };
            return View(model);
        }
    }
}
