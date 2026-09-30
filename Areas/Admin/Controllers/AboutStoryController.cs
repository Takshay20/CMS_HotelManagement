using CMS_HotelBooking.Services.Interfaces;
using CMS_HotelBooking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS_HotelBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AboutStoryController : Controller
    {
        private readonly IAboutStoryService _service;
        private readonly IWebHostEnvironment _environment;
        public AboutStoryController(IAboutStoryService service,IWebHostEnvironment environment)
        {
            _service = service;
            _environment = environment;
        }

        // About story
        [HttpGet]
        public async Task<IActionResult> AboutStory()
        {
            var data = (await _service.GetAllAsync())
                .OrderBy(x => x.DisplayOrder)
                .FirstOrDefault();
            var model = new AboutStoryVM();
            if (data != null)
            {
                model.AboutStory = data;
            }
            return View(model);
        }

        // Update about story record
        [HttpPost]
        public async Task<IActionResult> Update(AboutStoryVM model)
        {
            if (model.AboutStory == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Invalid data."
                });
            }
            if (model.ImageFile != null)
            {
                model.AboutStory.ImagePath =await UploadImage(model.ImageFile);
            }
            model.AboutStory.IsActive = true;
            model.AboutStory.DisplayOrder = 1;
            var result = await _service.SaveAsync( model.AboutStory);
            if (result > 0)
            {
                return Json(new
                {
                    success = true,
                    message = "Story Content updated successfully."
                });
            }
            return Json(new
            {
                success = false,
                message = "Unable to update Story Content."
            });
        }

        // Upload image and return saved path
        private async Task<string> UploadImage(IFormFile file)
        {
            string folderPath = Path.Combine(_environment.WebRootPath,
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

            using FileStream stream =new FileStream(
                    filePath,
                    FileMode.Create
                );
            await file.CopyToAsync(stream);

            return Path.Combine(
                "uploads",
                "about",
                fileName
            ).Replace("\\", "/");
        }
    }
}