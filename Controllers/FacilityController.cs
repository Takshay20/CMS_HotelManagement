using CMS_HotelBooking.Models;
using CMS_HotelBooking.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CMS_HotelBooking.Controllers
{
    public class FacilityViewModel
    {
        public List<Slider> Sliders { get; set; } = new();
        public List<Facility> Facilities { get; set; } = new();
    }

    public class FacilityController : Controller
    {
        private readonly ISliderService _sliderService;
        private readonly IFacilityService _facilityService;

        public FacilityController(ISliderService sliderService, IFacilityService facilityService)
        {
            _sliderService = sliderService;
            _facilityService = facilityService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new FacilityViewModel
            {
                Sliders = (await _sliderService.GetAllAsync("Facility"))
                            .Where(s => s.IsActive).OrderBy(s => s.DisplayOrder).ToList(),
                Facilities = (await _facilityService.GetAllAsync())
                            .Where(f => f.IsActive).OrderBy(f => f.DisplayOrder).ToList()
            };
            return View(model);
        }
    }
}
