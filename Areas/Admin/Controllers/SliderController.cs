using CMS_HotelBooking.Models;
using CMS_HotelBooking.Services.Interfaces;
using CMS_HotelBooking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS_HotelBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SliderController : Controller
    {
        private readonly ISliderService _service;
        private readonly IWebHostEnvironment _environment;

        public SliderController(
            ISliderService service,
            IWebHostEnvironment environment)
        {
            _service = service;
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Slider()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string? pageKey = null)
        {
            var result = await _service.GetAllAsync(pageKey);

            return Json(
                ResponseModel.SuccessResponse(
                    "Success",
                    result
                )
            );
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
            {
                return Json(
                    ResponseModel.ErrorResponse(
                        "Record not found."
                    )
                );
            }

            return Json(
                ResponseModel.SuccessResponse(
                    "Success",
                    result
                )
            );
        }

        [HttpPost]
        public async Task<IActionResult> Save(SliderVM model)
        {
            if (model.Slider == null)
            {
                return Json(
                    ResponseModel.ErrorResponse(
                        "Invalid data."
                    )
                );
            }

            if (model.Slider.SliderId == 0)
            {
                if (model.ImageFile == null)
                {
                    return Json(
                        ResponseModel.ErrorResponse(
                            "Slider image is required."
                        )
                    );
                }

                model.Slider.ImagePath =
                    await UploadImage(model.ImageFile);

                if (model.Slider.DisplayOrder <= 0)
                {
                    var records =
                        await _service.GetAllAsync(
                            model.Slider.PageKey
                        );

                    model.Slider.DisplayOrder =
                        records.Count > 0
                            ? records.Max(x => x.DisplayOrder) + 1
                            : 1;
                }

                var result =
                    await _service.SaveAsync(
                        model.Slider
                    );

                if (result > 0)
                {
                    return Json(
                        ResponseModel.SuccessResponse(
                            "Record Saved Successfully."
                        )
                    );
                }

                return Json(
                    ResponseModel.ErrorResponse(
                        "Unable To Save Record."
                    )
                );
            }

            var existing =
                await _service.GetByIdAsync(
                    model.Slider.SliderId
                );

            if (existing == null)
            {
                return Json(
                    ResponseModel.ErrorResponse(
                        "Record not found."
                    )
                );
            }

            if (model.ImageFile != null)
            {
                model.Slider.ImagePath =
                    await UploadImage(
                        model.ImageFile
                    );
            }
            else
            {
                model.Slider.ImagePath =
                    existing.ImagePath;
            }

            model.Slider.DisplayOrder =
                model.Slider.DisplayOrder > 0
                    ? model.Slider.DisplayOrder
                    : existing.DisplayOrder;

            var updateResult =
                await _service.SaveAsync(
                    model.Slider
                );

            if (updateResult > 0)
            {
                return Json(
                    ResponseModel.SuccessResponse(
                        "Record Updated Successfully."
                    )
                );
            }

            return Json(
                ResponseModel.ErrorResponse(
                    "Unable To Update Record."
                )
            );
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result =
                await _service.DeleteAsync(id);

            if (result > 0)
            {
                return Json(
                    ResponseModel.SuccessResponse(
                        "Record Deleted Successfully."
                    )
                );
            }

            return Json(
                ResponseModel.ErrorResponse(
                    "Unable To Delete Record."
                )
            );
        }

        private async Task<string> UploadImage(
            IFormFile file)
        {
            string folderPath =
                Path.Combine(
                    _environment.WebRootPath,
                    "uploads",
                    "slider"
                );

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(
                    folderPath
                );
            }

            string extension =
                Path.GetExtension(
                    file.FileName
                );

            string fileName =
                Guid.NewGuid().ToString()
                + extension;

            string filePath =
                Path.Combine(
                    folderPath,
                    fileName
                );

            using FileStream stream =
                new FileStream(
                    filePath,
                    FileMode.Create
                );

            await file.CopyToAsync(stream);

            return Path.Combine(
                "uploads",
                "slider",
                fileName
            ).Replace("\\", "/");
        }
    }
}