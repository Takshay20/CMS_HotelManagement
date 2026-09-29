using CMS_HotelBooking.Models;
using CMS_HotelBooking.Services.Interfaces;
using CMS_HotelBooking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS_HotelBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class GalleryController : Controller
    {
        private readonly IGalleryService _service;
        private readonly IWebHostEnvironment _environment;

        public GalleryController(
            IGalleryService service,
            IWebHostEnvironment environment)
        {
            _service = service;
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Gallery()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();

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

            return Json(ResponseModel.SuccessResponse( "Success", result));
        }
        [HttpPost]
        public async Task<IActionResult> Save(GalleryVM model)
        {
            if (model.Gallery == null)
            {
                return Json(
                    ResponseModel.ErrorResponse("Invalid Data" ));
            }

            bool isEdit =
                model.Gallery.GalleryId > 0;

            Gallery? existing = null;

            if (isEdit)
            {
                existing = await _service.GetByIdAsync(
                    model.Gallery.GalleryId
                );

                if (existing == null)
                {
                    return Json(
                        ResponseModel.ErrorResponse(
                            "Record not found."
                        )
                    );
                }
            }

            if (model.ImageFile != null)
            {
                model.Gallery.ImagePath =
                    await UploadImage(
                        model.ImageFile
                    );
            }
            else if (isEdit)
            {
                model.Gallery.ImagePath =
                    existing!.ImagePath;
            }
            else
            {
                return Json(
                    ResponseModel.ErrorResponse(
                        "Please choose an image."
                    )
                );
            }

            if (!isEdit)
            {
                var existingRecords =
                    await _service.GetAllAsync();

                model.Gallery.DisplayOrder =
                    existingRecords.Count > 0
                        ? existingRecords.Max(
                            x => x.DisplayOrder
                        ) + 1
                        : 1;
            }
            else
            {
                model.Gallery.DisplayOrder =
                    existing!.DisplayOrder;
            }

            var result =
                await _service.SaveAsync(
                    model.Gallery
                );

            if (result == -1)
            {
                return Json(
                    ResponseModel.ErrorResponse(
                        "This title already exists."
                    )
                );
            }

            if (result > 0)
            {
                string message =
                    isEdit
                        ? "Record Updated Successfully."
                        : "Record Saved Successfully.";

                return Json(
                    ResponseModel.SuccessResponse(
                        message
                    )
                );
            }

            return Json(
                ResponseModel.ErrorResponse(
                    "Unable To Save Record"
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
                    "Unable To Delete Record"
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
                    "gallery"
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
                "gallery",
                fileName
            ).Replace(
                "\\",
                "/"
            );
        }
    }
}