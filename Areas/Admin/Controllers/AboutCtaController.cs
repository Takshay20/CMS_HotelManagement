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
    public class AboutCtaController : Controller
    {
        private readonly IAboutCtaService _service;
        private readonly IWebHostEnvironment _environment;

        public AboutCtaController(IAboutCtaService service, IWebHostEnvironment environment)
        {
            _service = service;
            _environment = environment;
        }


        // About cta
        [HttpGet]
        public async Task<IActionResult> AboutCta()
        {
            var data = (await _service.GetAllAsync())
                .FirstOrDefault();

            var model = new AboutCtaVM();

            if (data != null)
            {
                model.AboutCta = data;
            }

            return View(model);
        }

        // Update about cta record
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Update(AboutCtaVM model)
        {
            if (model.AboutCta == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Invalid data."
                });
            }

            if (model.ImageFile != null)
            {
                model.AboutCta.ImagePath = await UploadImage(model.ImageFile);
            }

            var result = await _service.SaveAsync(model.AboutCta);

            if (result > 0)
            {
                return Json(new
                {
                    success = true,
                    message = "CTA updated successfully."
                });
            }

            return Json(new
            {
                success = false,
                message = "Unable to update CTA."
            });
        }

        // Upload image and return saved path
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
