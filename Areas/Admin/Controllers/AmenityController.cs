using CMS_HotelBooking.Models;
using CMS_HotelBooking.Services.Interfaces;
using CMS_HotelBooking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS_HotelBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AmenityController : Controller
    {
        private readonly IAmenityService _service;
        public AmenityController(IAmenityService service)
        {
            _service = service;
        }

        // Open amenity page
        public IActionResult Amenity()
        {
            return View();
        }

        // Get all amenity records
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Json(ResponseModel.SuccessResponse("Success", result));
        }

        // Get amenity record by id
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return Json(ResponseModel.ErrorResponse("Record not found."));
            return Json(ResponseModel.SuccessResponse("Success", result));
        }

        // Save amenity record
        [HttpPost]
        public async Task<IActionResult> Save(AmenityVM model)
        {
            if (model.Amenity == null)
                return Json(ResponseModel.ErrorResponse("Invalid Data"));
            var result = await _service.SaveAsync(model.Amenity);
            if (result > 0)
            {
                string message = model.Amenity.AmenityId == 0 ? "Record Saved Successfully." : "Record Updated Successfully.";
                return Json(ResponseModel.SuccessResponse(message));
            }
            return Json(ResponseModel.ErrorResponse("Unable To Save Record"));
        }

        // Delete amenity record
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (result > 0)
                return Json(ResponseModel.SuccessResponse("Record Deleted Successfully."));
            return Json(ResponseModel.ErrorResponse("Unable To Delete Record"));
        }
    }
}
