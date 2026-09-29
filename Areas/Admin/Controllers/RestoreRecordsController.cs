using CMS_HotelBooking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS_HotelBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RestoreRecordsController : Controller
    {
        private readonly IHomeWhyChooseUsService _whyChooseUsService;
        private readonly IAboutCounterService _aboutCounterService;

        public RestoreRecordsController(
            IHomeWhyChooseUsService whyChooseUsService,
            IAboutCounterService aboutCounterService)
        {
            _whyChooseUsService = whyChooseUsService;
            _aboutCounterService = aboutCounterService;
        }

        [HttpGet]
        public IActionResult RestoreRecords()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetWhyChooseUs(
            string filter = "All")
        {
            var result =
                await _whyChooseUsService.GetAllRecordsAsync(filter);

            return Json(new
            {
                success = true,
                data = result
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetCounterBox(
            string filter = "All")
        {
            var result =
                await _aboutCounterService.GetAllRecordsAsync(filter);

            return Json(new
            {
                success = true,
                data = result
            });
        }
    }
}