using CMS_HotelBooking.Models;
using CMS_HotelBooking.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CMS_HotelBooking.ViewComponents
{
    public class FooterViewModel
    {
        public Footer Footer { get; set; } = new Footer();
        public List<MenuMaster> Menu { get; set; } = new();
        public List<SocialMedia> SocialLinks { get; set; } = new();
    }

    public class FooterViewComponent : ViewComponent
    {
        private readonly IFooterService _footerService;
        private readonly IMenuMasterService _menuMasterService;
        private readonly ISocialMediaService _socialMediaService;

        public FooterViewComponent(IFooterService footerService, IMenuMasterService menuMasterService, ISocialMediaService socialMediaService)
        {
            _footerService = footerService;
            _menuMasterService = menuMasterService;
            _socialMediaService = socialMediaService;
        }

        // Load footer data for the view
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new FooterViewModel
            {
                Footer = await _footerService.GetAsync() ?? new Footer(),
                Menu = await _menuMasterService.GetActiveAsync(),
                SocialLinks = await _socialMediaService.GetActiveAsync()
            };
            return View(model);
        }
    }
}
