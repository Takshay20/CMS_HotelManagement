using CMS_HotelBooking.Models;
using CMS_HotelBooking.Services.Interfaces;
using CMS_HotelBooking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS_HotelBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SocialMediaController : Controller
    {
        private readonly ISocialMediaService _service;

        public SocialMediaController(ISocialMediaService service)
        {
            _service = service;
        }

        // Open social media page
        public IActionResult SocialMedia()
        {
            return View();
        }

        // Get all social media records
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Json(ResponseModel.SuccessResponse("Success", result));
        }

        // Get social media record by id
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return Json(ResponseModel.ErrorResponse("Record not found."));
            return Json(ResponseModel.SuccessResponse("Success", result));
        }

        // Save social media record
        [HttpPost]
        public async Task<IActionResult> Save(SocialMediaVM model)
        {
            if (model.SocialMedia == null)
                return Json(ResponseModel.ErrorResponse("Invalid Data"));

                        if (model.SocialMedia.SocialMediaId == 0)
            {
                var existingRecords = await _service.GetAllAsync();
                model.SocialMedia.DisplayOrder = existingRecords.Count > 0 ? existingRecords.Max(x => x.DisplayOrder) + 1 : 1;
            }
            else
            {
                var existing = await _service.GetByIdAsync(model.SocialMedia.SocialMediaId);
                if (existing != null)
                    model.SocialMedia.DisplayOrder = existing.DisplayOrder;
            }

            var result = await _service.SaveAsync(model.SocialMedia);

            if (result > 0)
            {
                string message = model.SocialMedia.SocialMediaId == 0 ? "Record Saved Successfully." : "Record Updated Successfully.";
                return Json(ResponseModel.SuccessResponse(message));
            }
            return Json(ResponseModel.ErrorResponse("Unable To Save Record"));
        }

        // Delete social media record
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
