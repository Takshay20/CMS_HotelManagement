using CMS_HotelBooking.Models;
using CMS_HotelBooking.Services.Interfaces;
using CMS_HotelBooking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CMS_HotelBooking.Filters;

namespace CMS_HotelBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeWhyChooseUsController : Controller
    {
        private readonly IHomeWhyChooseUsService _service;
        public HomeWhyChooseUsController(IHomeWhyChooseUsService service)
        {
            _service = service;
        }

        // Open home why choose us page
        public IActionResult HomeWhyChooseUs()
        {
            return View();
        }

        // Open all records page
        [HttpGet]
        public IActionResult AllRecords()
        {
            return View();
        }

        // Get all home why choose us records
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Json(ResponseModel.SuccessResponse("Success", result));
        }

        // Get home why choose us record by id
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
            {
                return Json(ResponseModel.ErrorResponse("Record not found."));
            }

            return Json(ResponseModel.SuccessResponse("Success", result));
        }

        // Save home why choose us record
        [HttpPost]
        public async Task<IActionResult> Save(HomeWhyChooseUsVM model)
        {
            if (model.HomeWhyChooseUs == null)
            {
                return Json( ResponseModel.ErrorResponse("Invalid Data"));
            }

            if (model.HomeWhyChooseUs.HomeWhyChooseUsId == 0)
            {
                var existingRecords = await _service.GetAllAsync();
                model.HomeWhyChooseUs.DisplayOrder = existingRecords.Count > 0 ? existingRecords.Max(x => x.DisplayOrder) + 1 : 1;
            }
            else
            {
                var existing = await _service.GetByIdAsync(model.HomeWhyChooseUs.HomeWhyChooseUsId);

                if (existing != null)
                {
                    model.HomeWhyChooseUs.DisplayOrder = existing.DisplayOrder;
                }
            }
            var result = await _service.SaveAsync(model.HomeWhyChooseUs);

            if (result > 0)
            {
                string message = model.HomeWhyChooseUs.HomeWhyChooseUsId == 0 ? "Record Saved Successfully." : "Record Updated Successfully.";

                return Json(ResponseModel.SuccessResponse(message));
            }

            return Json(ResponseModel.ErrorResponse("Unable To Save Record"));
        }

        // Delete home why choose us record
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);

            if (result > 0)
            {
                return Json(ResponseModel.SuccessResponse("Record Deleted Successfully."));
            }

            return Json(ResponseModel.ErrorResponse("Unable To Delete Record"));
        }

        // Get all records
        [HttpGet]
        public async Task<IActionResult> GetAllRecords(string filter = "All")
        {
            var result = await _service.GetAllRecordsAsync(filter);

            return Json(ResponseModel.SuccessResponse("Success",result));
        }

        // Restore deleted home why choose us record
        [ServiceFilter(typeof(RestoreRestrictionFilter))]
        [HttpPost]
        public async Task<IActionResult> Restore(int id)
        {
            try
            {
                var result = await _service.RestoreAsync(id);

                if (result > 0)
                {
                    return Json(ResponseModel.SuccessResponse("Record restored successfully.",result));
                }

                return Json(ResponseModel.ErrorResponse("Record could not be restored." ));
            }
            catch (Exception ex)
            {
                return Json(ResponseModel.ErrorResponse(ex.Message));
            }
        }
    }
}