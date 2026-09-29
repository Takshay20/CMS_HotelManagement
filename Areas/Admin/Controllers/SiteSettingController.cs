using CMS_HotelBooking.Models;
using CMS_HotelBooking.Services.Interfaces;
using CMS_HotelBooking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS_HotelBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SiteSettingController : Controller
    {
        private readonly ISiteSettingService _service;
        private readonly IWebHostEnvironment _environment;

        public SiteSettingController(ISiteSettingService service, IWebHostEnvironment environment)
        {
            _service = service;
            _environment = environment;
        }

        public IActionResult SiteSetting()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _service.GetAsync();
            return Json(ResponseModel.SuccessResponse("Success", result));
        }

        [HttpPost]
        public async Task<IActionResult> Save(SiteSettingVM model)
        {
            if (model.SiteSetting == null)
                return Json(ResponseModel.ErrorResponse("Invalid Data"));

            if (model.LogoFile != null)
                model.SiteSetting.LogoPath = await UploadImage(model.LogoFile);

            var result = await _service.SaveAsync(model.SiteSetting);

            if (result > 0)
                return Json(ResponseModel.SuccessResponse("Site Settings Updated Successfully."));
            return Json(ResponseModel.ErrorResponse("Unable To Save Record"));
        }

        private async Task<string> UploadImage(IFormFile file)
        {
            string folderPath = Path.Combine(_environment.WebRootPath, "uploads", "settings");
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(folderPath, fileName);
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return Path.Combine("uploads", "settings", fileName).Replace("\\", "/");
        }
    }
}
