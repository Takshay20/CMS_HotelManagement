using CMS_HotelBooking.Models;
using CMS_HotelBooking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS_HotelBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RestoreRecordsController : Controller
    {
        private readonly IRestoreRecordsService _service;

        public RestoreRecordsController(IRestoreRecordsService service)
        {
            _service = service;
        }

        // Open restore records page
        [HttpGet]
        public IActionResult RestoreRecords()
        {
            return View();
        }

        // Get records
        [HttpGet]
        public async Task<IActionResult> GetRecords(string module, string filter = "All")
        {
            var result = await _service.GetRecordsAsync(module, filter);

            return Json(ResponseModel.SuccessResponse("Success", result));
        }

        // Restore deleted restore record
        [HttpPost]
        public async Task<IActionResult> Restore(string module, int id)
        {
            var result = await _service.RestoreAsync(module, id);

            if (result > 0)
            {
                return Json(ResponseModel.SuccessResponse("Record restored successfully."));
            }

            return Json(ResponseModel.ErrorResponse("This record can be restored only after 1 hour."));
        }
    }
}
