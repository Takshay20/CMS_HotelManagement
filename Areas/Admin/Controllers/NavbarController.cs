using CMS_HotelBooking.Models;
using CMS_HotelBooking.Services.Interfaces;
using CMS_HotelBooking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS_HotelBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class NavbarController : Controller
    {
        private readonly IMenuMasterService _service;

        public NavbarController(IMenuMasterService service)
        {
            _service = service;
        }

        // Open navbar page
        public IActionResult Navbar()
        {
            return View();
        }

        // Get all navbar records
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Json(ResponseModel.SuccessResponse("Success", result));
        }

        // Get navbar record by id
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return Json(ResponseModel.ErrorResponse("Record not found."));
            return Json(ResponseModel.SuccessResponse("Success", result));
        }

        // Save navbar record
        [HttpPost]
        public async Task<IActionResult> Save(MenuMasterVM model)
        {
            if (model.MenuMaster == null)
                return Json(ResponseModel.ErrorResponse("Invalid Data"));

                        if (model.MenuMaster.MenuMasterId == 0)
            {
                var existingRecords = await _service.GetAllAsync();
                model.MenuMaster.DisplayOrder = existingRecords.Count > 0 ? existingRecords.Max(x => x.DisplayOrder) + 1 : 1;
            }
            else
            {
                var existing = await _service.GetByIdAsync(model.MenuMaster.MenuMasterId);
                if (existing != null)
                    model.MenuMaster.DisplayOrder = existing.DisplayOrder;
            }

            var result = await _service.SaveAsync(model.MenuMaster);

            if (result > 0)
            {
                string message = model.MenuMaster.MenuMasterId == 0 ? "Record Saved Successfully." : "Record Updated Successfully.";
                return Json(ResponseModel.SuccessResponse(message));
            }
            return Json(ResponseModel.ErrorResponse("Unable To Save Record"));
        }

        // Delete navbar record
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
