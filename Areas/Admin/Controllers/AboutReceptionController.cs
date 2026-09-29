using CMS_HotelBooking.Models;
using CMS_HotelBooking.Services.Interfaces;
using CMS_HotelBooking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CMS_HotelBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AboutReceptionController : Controller
    {
        private readonly IAboutReceptionService _service;
        private readonly IWebHostEnvironment _environment;
        public AboutReceptionController(IAboutReceptionService service, IWebHostEnvironment environment)
        {
            _service = service;
            _environment = environment;
        }
        [HttpGet]
        public async Task<IActionResult> AboutReception()
        {
            var data = (await _service.GetAllAsync())
                .OrderBy(x => x.DisplayOrder)
                .FirstOrDefault();
            var model = new AboutReceptionVM();
            if (data != null)
            {
                model.AboutReception = data;
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Update(AboutReceptionVM model)
        {
            if (model.AboutReception == null)
            {
                return Json(new { success = false, message = "Invalid data." });
            }
            if (string.IsNullOrWhiteSpace(model.AboutReception.Title))
            {
                return Json(new { success = false, message = "Title is required." });
            }
            if (string.IsNullOrWhiteSpace(model.AboutReception.Description))
            {
                return Json(new { success = false, message = "Description is required." });
            }
            if (model.ImageFile != null)
            {
                model.AboutReception.ImagePath = await UploadImage(model.ImageFile);
            }
            model.AboutReception.IsActive = true;
            model.AboutReception.DisplayOrder = 1;
            var result = await _service.SaveAsync(model.AboutReception);
            if (result > 0)
            {
                return Json(new { success = true, message = "Reception Content updated successfully." });
            }
            return Json(new { success = false, message = "Unable to update Reception Content." });
        }
        private async Task<string> UploadImage(IFormFile file)
        {
            string folderPath = Path.Combine(_environment.WebRootPath, "uploads", "about");
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(folderPath, fileName);
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return Path.Combine("uploads", "about", fileName).Replace("\\", "/");
        }
    }
}
