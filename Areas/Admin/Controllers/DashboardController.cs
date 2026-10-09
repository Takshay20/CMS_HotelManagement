using CMS_HotelBooking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS_HotelBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _service;
        public DashboardController(IDashboardService service)
        {
            _service = service;
        }

        // Open dashboard page
        public IActionResult Dashboard()
        {
            return View();
        }

        // Get counts
        [HttpGet]
        public async Task<IActionResult> GetCounts()
        {
            var result = await _service.GetCountsAsync();
            return Json(Models.ResponseModel.SuccessResponse("Success", result));
        }

        // Get reminders
        [HttpGet]
        public async Task<IActionResult> GetReminders()
        {
            var result = await _service.GetUpcomingArrivalsDeparturesAsync();
            return Json(Models.ResponseModel.SuccessResponse("Success", result));
        }
    }
}
