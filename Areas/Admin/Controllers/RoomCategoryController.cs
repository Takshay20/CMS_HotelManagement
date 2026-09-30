using CMS_HotelBooking.Models;
using CMS_HotelBooking.Services.Interfaces;
using CMS_HotelBooking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS_HotelBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RoomCategoryController : Controller
    {
        private readonly IRoomCategoryService _service;

        public RoomCategoryController(IRoomCategoryService service)
        {
            _service = service;
        }

        // Open room category page
        public IActionResult RoomCategory()
        {
            return View();
        }

        // Get all room category records
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Json(ResponseModel.SuccessResponse("Success", result));
        }

        // Get room category record by id
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return Json(ResponseModel.ErrorResponse("Record not found."));
            return Json(ResponseModel.SuccessResponse("Success", result));
        }

        // Save room category record
        [HttpPost]
        public async Task<IActionResult> Save(RoomCategoryVM model)
        {
            try
            {
                if (model.RoomCategory == null)
                    return Json(ResponseModel.ErrorResponse("Invalid Data"));

                if (model.RoomCategory.RoomCategoryId == 0)
                {
                    var existingRecords = await _service.GetAllAsync();

                    model.RoomCategory.DisplayOrder =
                        existingRecords.Count > 0
                        ? existingRecords.Max(x => x.DisplayOrder) + 1
                        : 1;
                }
                else
                {
                    var existing = await _service.GetByIdAsync(model.RoomCategory.RoomCategoryId);
                    if (existing != null)
                        model.RoomCategory.DisplayOrder = existing.DisplayOrder;
                }

                var result = await _service.SaveAsync(model.RoomCategory);

                if (result > 0)
                {
                    string message = model.RoomCategory.RoomCategoryId == 0
                        ? "Record Saved Successfully."
                        : "Record Updated Successfully.";

                    return Json(ResponseModel.SuccessResponse(message));
                }

                return Json(ResponseModel.ErrorResponse("Unable To Save Record"));
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("duplicate") || ex.Message.Contains("UNIQUE"))
                {
                    return Json(ResponseModel.ErrorResponse(
                        "Room Category already exists."
                    ));
                }

                return Json(ResponseModel.ErrorResponse(
                    "Unable To Save Record"
                ));
            }
        }


        // Delete room category record
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
