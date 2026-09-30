using CMS_HotelBooking.Services.Interfaces;
using CMS_HotelBooking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS_HotelBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AboutController : Controller
    {
        private readonly IAboutService _service;
        private readonly IWebHostEnvironment _environment;

        public AboutController( IAboutService service,IWebHostEnvironment environment)
        {
            _service = service;
            _environment = environment;
        }

        // Open about page
        [HttpGet]
        public async Task<IActionResult> About()
        {
            var data = await _service.GetAsync();

            var model = new AboutVM();

            if (data != null)
            {
                model.About = data;
            }

            return View(model);
        }

        // Update about record
        [HttpPost]
        public async Task<IActionResult> Update(AboutVM model)
        {
            if (model.About == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Invalid data."
                });
            }

            if (model.Image1File != null)
            {
                model.About.Image =
                    await UploadImage(model.Image1File);
            }

            var result = await _service.SaveAsync(
                model.About
            );

            if (result > 0)
            {
                return Json(new
                {
                    success = true,
                    message = "About Page updated successfully."
                });
            }

            return Json(new
            {
                success = false,
                message = "Unable to update About Page."
            });
        }

        // Upload image and return saved path
        private async Task<string> UploadImage(IFormFile file)
        {
            string folderPath = Path.Combine( _environment.WebRootPath,
                "uploads",
                "about"
            );

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string extension =
                Path.GetExtension(file.FileName);

            string fileName =
                Guid.NewGuid().ToString() + extension;

            string filePath =
                Path.Combine(folderPath, fileName);

            using FileStream stream =new FileStream(filePath,FileMode.Create);

            await file.CopyToAsync(stream);

            return Path.Combine(
                "uploads",
                "about",
                fileName
            ).Replace("\\", "/");
        }
    }
}