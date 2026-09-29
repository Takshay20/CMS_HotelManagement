using CMS_HotelBooking.Services.Interfaces;
using CMS_HotelBooking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS_HotelBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeWelcomeController : Controller
    {
        private readonly IHomeWelcomeService _homeWelcomeService;
        private readonly IWebHostEnvironment _environment;

        public HomeWelcomeController(
            IHomeWelcomeService homeWelcomeService,
            IWebHostEnvironment environment)
        {
            _homeWelcomeService = homeWelcomeService;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> HomeWelcome()
        {
            var data = await _homeWelcomeService.GetAsync();

            var model = new HomeWelcomeVM();

            if (data != null)
            {
                model.HomeWelcome = data;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(HomeWelcomeVM model)
        {
            if (model.HomeWelcome == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Invalid data."
                });
            }

            if (model.Image1File != null)
            {
                model.HomeWelcome.Image1Path =
                    await UploadImage(model.Image1File);
            }

            if (model.Image2File != null)
            {
                model.HomeWelcome.Image2Path =
                    await UploadImage(model.Image2File);
            }

            if (model.Image3File != null)
            {
                model.HomeWelcome.Image3Path =
                    await UploadImage(model.Image3File);
            }

            model.HomeWelcome.DisplayOrder = 1;

            var result = await _homeWelcomeService.UpdateAsync(
                model.HomeWelcome
            );

            if (result > 0)
            {
                return Json(new
                {
                    success = true,
                    message = "Home Welcome updated successfully."
                });
            }

            return Json(new
            {
                success = false,
                message = "Unable to update Home Welcome."
            });
        }

        private async Task<string> UploadImage(IFormFile file)
        {
            string folderPath = Path.Combine(
                _environment.WebRootPath,
                "uploads",
                "home"
            );

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string extension = Path.GetExtension(file.FileName);

            string fileName =
                Guid.NewGuid().ToString() + extension;

            string filePath =
                Path.Combine(folderPath, fileName);

            using FileStream stream =
                new FileStream(filePath, FileMode.Create);

            await file.CopyToAsync(stream);

            return Path.Combine(
                "uploads",
                "home",
                fileName
            ).Replace("\\", "/");
        }
    }
}