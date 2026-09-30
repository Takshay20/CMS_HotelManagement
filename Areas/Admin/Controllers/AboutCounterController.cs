using CMS_HotelBooking.Models;
using CMS_HotelBooking.Services.Interfaces;
using CMS_HotelBooking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS_HotelBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AboutCounterController : Controller
    {
        private readonly IAboutCounterService _service;

        public AboutCounterController(IAboutCounterService service)
        {
            _service = service;
        }

        // Open about counter page
        public IActionResult AboutCounter()
        {
            return View();
        }

        // Get all about counter records
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Json(ResponseModel.SuccessResponse("Success", result));
        }

        // Get about counter record by id
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return Json(ResponseModel.ErrorResponse("Record not found."));
            return Json(ResponseModel.SuccessResponse("Success", result));
        }

        // Save about counter record
        [HttpPost]
        public async Task<IActionResult> Save(AboutCounterVM model)
        {
            if (model.AboutCounter == null)
                return Json(ResponseModel.ErrorResponse("Invalid Data"));

                        if (model.AboutCounter.AboutCounterId == 0)
            {
                var existingRecords = await _service.GetAllAsync();
                model.AboutCounter.DisplayOrder = existingRecords.Count > 0 ? existingRecords.Max(x => x.DisplayOrder) + 1 : 1;
            }
            else
            {
                var existing = await _service.GetByIdAsync(model.AboutCounter.AboutCounterId);
                if (existing != null)
                    model.AboutCounter.DisplayOrder = existing.DisplayOrder;
            }

            var result = await _service.SaveAsync(model.AboutCounter);

            if (result > 0)
            {
                string message = model.AboutCounter.AboutCounterId == 0 ? "Record Saved Successfully." : "Record Updated Successfully.";
                return Json(ResponseModel.SuccessResponse(message));
            }
            return Json(ResponseModel.ErrorResponse("Unable To Save Record"));
        }

        // Delete about counter record
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (result > 0)
                return Json(ResponseModel.SuccessResponse("Record Deleted Successfully."));
            return Json(ResponseModel.ErrorResponse("Unable To Delete Record"));
        }

        // Restore deleted about counter record
        [HttpPost]
        public async Task<IActionResult> Restore(int id)
        {
            var result = await _service.RestoreAsync(id);
            if (result > 0)
                return Json(ResponseModel.SuccessResponse("Record Restored Successfully."));
            return Json(ResponseModel.ErrorResponse("Unable To Restore Record"));
        }
    }
}
