using CMS_HotelBooking.Models;
using CMS_HotelBooking.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CMS_HotelBooking.Controllers
{
    public class AboutViewModel
    {
        public List<Slider> Sliders { get; set; } = new();
        public About? About { get; set; }
        public List<Facility> Facilities { get; set; } = new();
        public List<HomeWhyChooseUs> WhyChooseUs { get; set; } = new();
        public List<Gallery> GalleryImages { get; set; } = new();
        public List<AboutStory> Stories { get; set; } = new();
        public List<AboutReception> Receptions { get; set; } = new();
        public List<AboutCounter> Counters { get; set; } = new();
        public AboutCta? Cta { get; set; }
    }

    public class AboutController : Controller
    {
        private readonly ISliderService _sliderService;
        private readonly IAboutService _aboutService;
        private readonly IFacilityService _facilityService;
        private readonly IHomeWhyChooseUsService _whyChooseUsService;
        private readonly IGalleryService _galleryService;
        private readonly IAboutStoryService _aboutStoryService;
        private readonly IAboutReceptionService _aboutReceptionService;
        private readonly IAboutCounterService _aboutCounterService;
        private readonly IAboutCtaService _aboutCtaService;

        public AboutController(
            ISliderService sliderService,
            IAboutService aboutService,
            IFacilityService facilityService,
            IHomeWhyChooseUsService whyChooseUsService,
            IGalleryService galleryService,
            IAboutStoryService aboutStoryService,
            IAboutReceptionService aboutReceptionService,
            IAboutCounterService aboutCounterService,
            IAboutCtaService aboutCtaService)
        {
            _sliderService = sliderService;
            _aboutService = aboutService;
            _facilityService = facilityService;
            _whyChooseUsService = whyChooseUsService;
            _galleryService = galleryService;
            _aboutStoryService = aboutStoryService;
            _aboutReceptionService = aboutReceptionService;
            _aboutCounterService = aboutCounterService;
            _aboutCtaService = aboutCtaService;
        }

        // Open about page
        public async Task<IActionResult> Index()
        {
            var model = new AboutViewModel
            {
                Sliders = await _sliderService.GetAllAsync("About"),
                About = await _aboutService.GetAsync(),
                Facilities = (await _facilityService.GetAllAsync()).Where(f => f.IsActive).OrderBy(f => f.DisplayOrder).ToList(),
                WhyChooseUs = (await _whyChooseUsService.GetAllAsync()).Where(w => w.IsActive).OrderBy(w => w.DisplayOrder).ToList(),
                GalleryImages = (await _galleryService.GetAllAsync()).Where(g => g.IsActive).OrderBy(g => g.DisplayOrder).ToList(),
                Stories = (await _aboutStoryService.GetAllAsync()).Where(s => s.IsActive).OrderBy(s => s.DisplayOrder).ToList(),
                Receptions = (await _aboutReceptionService.GetAllAsync()).Where(r => r.IsActive).OrderBy(r => r.DisplayOrder).ToList(),
                Counters = (await _aboutCounterService.GetAllAsync()).Where(c => c.IsActive).OrderBy(c => c.DisplayOrder).ToList(),
                Cta = (await _aboutCtaService.GetAllAsync()).FirstOrDefault()
            };
            return View(model);
        }
    }
}
