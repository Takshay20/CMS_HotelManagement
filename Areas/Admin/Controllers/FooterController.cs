using CMS_HotelBooking.Models;
using CMS_HotelBooking.Services.Interfaces;
using CMS_HotelBooking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS_HotelBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class FooterController : Controller
    {
        private readonly IFooterService _service;

        public FooterController(IFooterService service)
        {
            _service = service;
        }

        // Open footer page
        public IActionResult Footer()
        {
            return View();
        }

        // Get footer record
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _service.GetAsync();
            return Json(ResponseModel.SuccessResponse("Success", result));
        }

        // Save footer record
        [HttpPost]
        public async Task<IActionResult> Save(FooterVM model)
        {
            if (model.Footer == null)
                return Json(ResponseModel.ErrorResponse("Invalid Data"));

            var result = await _service.SaveAsync(model.Footer);

            if (result > 0)
                return Json(ResponseModel.SuccessResponse("Footer Updated Successfully."));
            return Json(ResponseModel.ErrorResponse("Unable To Save Record"));
        }
    }
}
