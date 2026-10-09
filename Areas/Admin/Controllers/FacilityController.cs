using CMS_HotelBooking.Models;
using CMS_HotelBooking.Services.Interfaces;
using CMS_HotelBooking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS_HotelBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class FacilityController : Controller
    {
        private readonly IFacilityService _service;
        private readonly IWebHostEnvironment _environment;

        public FacilityController(IFacilityService service,IWebHostEnvironment environment)
        {
            _service = service;
            _environment = environment;
        }

        // Open facility page
        [HttpGet]
        public IActionResult Facility()
        {
            return View();
        }

        // Get all facility records
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();

            return Json(ResponseModel.SuccessResponse("Success",result));
        }

        // Get facility record by id
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
            {
                return Json(ResponseModel.ErrorResponse("Record not found."));
            }

            return Json(ResponseModel.SuccessResponse("Success",result));
        }

        // Save facility record
        [HttpPost]
        public async Task<IActionResult> Save(FacilityVM model)
        {
            if (model.Facility == null)
            {
                return Json(ResponseModel.ErrorResponse("Invalid Data"));
            }

            bool isEdit = model.Facility.FacilityId > 0;

            Facility? existing = null;

            if (isEdit)
            {
                existing = await _service.GetByIdAsync(model.Facility.FacilityId);

                if (existing == null)
                {
                    return Json(ResponseModel.ErrorResponse("Record not found."));
                }
            }

            if (model.ImageFile != null)
            {
                model.Facility.ImagePath = await UploadImage(model.ImageFile);
            }
            else if (isEdit)
            {
                model.Facility.ImagePath = existing!.ImagePath;
            }
            else
            {
                return Json(ResponseModel.ErrorResponse("Please choose an image."));
            }

            if (!isEdit)
            {
                var existingRecords = await _service.GetAllAsync();
                model.Facility.DisplayOrder = existingRecords.Count > 0 ? existingRecords.Max(x => x.DisplayOrder ) + 1 : 1;
            }
            else
            {
                model.Facility.DisplayOrder = existing!.DisplayOrder;
            }

            var result = await _service.SaveAsync(model.Facility);

            if (result == -1)
            {
                return Json(ResponseModel.ErrorResponse("This title already exists."));
            }

            if (result > 0)
            {
                string message = isEdit ? "Record Updated Successfully.": "Record Saved Successfully.";

            }
            return Json(ResponseModel.ErrorResponse( "Unable To Save Record"));
        }

        // Delete facility record
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

        // Upload image and return saved path
        private async Task<string> UploadImage(IFormFile file)
        {
            string folderPath =Path.Combine( _environment.WebRootPath,"uploads", "facility");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string extension = Path.GetExtension(file.FileName);
            string fileName =Guid.NewGuid().ToString()+ extension;
            string filePath =Path.Combine(folderPath,fileName);
            using FileStream stream =new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);
            return Path.Combine("uploads","facility",fileName).Replace("\\", "/");
        }
    }
}