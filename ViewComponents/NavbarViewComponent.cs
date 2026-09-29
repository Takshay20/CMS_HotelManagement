using CMS_HotelBooking.Models;
using CMS_HotelBooking.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CMS_HotelBooking.ViewComponents
{
    public class NavbarViewModel
    {
        public SiteSetting SiteSetting { get; set; } = new SiteSetting();
        public List<MenuMaster> Menu { get; set; } = new();
    }

    public class NavbarViewComponent : ViewComponent
    {
        private readonly ISiteSettingService _siteSettingService;
        private readonly IMenuMasterService _menuMasterService;

        public NavbarViewComponent(ISiteSettingService siteSettingService, IMenuMasterService menuMasterService)
        {
            _siteSettingService = siteSettingService;
            _menuMasterService = menuMasterService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new NavbarViewModel
            {
                SiteSetting = await _siteSettingService.GetAsync() ?? new SiteSetting(),
                Menu = await _menuMasterService.GetActiveAsync()
            };
            return View(model);
        }
    }
}
