using CMS_HotelBooking.Models;
using CMS_HotelBooking.Services.Interfaces;
using CMS_HotelBooking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS_HotelBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RoomController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly IRoomCategoryService _categoryService;
        private readonly IAmenityService _amenityService;
        private readonly IWebHostEnvironment _environment;

        public RoomController(IRoomService roomService,IRoomCategoryService categoryService,IAmenityService amenityService,IWebHostEnvironment environment)
        {
            _roomService = roomService;
            _categoryService = categoryService;
            _amenityService = amenityService;
            _environment = environment;
        }

        // Open room page
        public IActionResult Room()
        {
            return View();
        }

        // Get all room records
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _roomService.GetAllAsync();
            return Json(ResponseModel.SuccessResponse("Success", result));
        }

        // Get room record by id
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _roomService.GetByIdAsync(id);
            if (result == null)
            return Json(ResponseModel.ErrorResponse("Record not found."));
            return Json(ResponseModel.SuccessResponse("Success", result));
        }

        // Get form data
        [HttpGet]
        public async Task<IActionResult> GetFormData()
        {
            var categories = await _categoryService.GetAllAsync();
            var amenities = await _amenityService.GetAllAsync();
            return Json(ResponseModel.SuccessResponse("Success", new { categories, amenities }));
        }

        // Save room record
        [HttpPost]
        public async Task<IActionResult> Save(RoomVM model)
        {
            if (model.Room == null)
                return Json(ResponseModel.ErrorResponse("Invalid Data"));

            if (model.ImageFile != null)
                model.Room.ImagePath = await UploadImage(model.ImageFile);
            else if (model.Room.RoomId == 0)
                return Json(ResponseModel.ErrorResponse("Please choose a cover image."));

            if (model.Room.RoomId == 0)
            {
                var existingRooms = await _roomService.GetAllAsync();
                model.Room.DisplayOrder = existingRooms.Count > 0 ? existingRooms.Max(x => x.DisplayOrder) + 1 : 1;
            }
            else
            {
                var existingRoom = await _roomService.GetByIdAsync(model.Room.RoomId);
                if (existingRoom != null)
                    model.Room.DisplayOrder = existingRoom.DisplayOrder;
            }

            var roomId = await _roomService.SaveAsync(model.Room);

            if (roomId > 0)
            {
                if (model.GalleryFiles != null && model.GalleryFiles.Count > 0)
                {
                    int order = 1;
                    foreach (var file in model.GalleryFiles)
                    {
                        var path = await UploadImage(file);
                        await _roomService.AddImageAsync(roomId, path, order++);
                    }
                }

                string message = model.Room.RoomId == 0 ? "Record Saved Successfully." : "Record Updated Successfully.";
                return Json(ResponseModel.SuccessResponse(message));
            }
            return Json(ResponseModel.ErrorResponse("Unable To Save Record"));
        }

        // Delete room record
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _roomService.DeleteAsync(id);
            if (result > 0)
                return Json(ResponseModel.SuccessResponse("Record Deleted Successfully."));
                return Json(ResponseModel.ErrorResponse("Unable To Delete Record"));
        }

        // Delete image
        [HttpPost]
        public async Task<IActionResult> DeleteImage(int roomImageId)
        {
            var result = await _roomService.DeleteImageAsync(roomImageId);
            if (result > 0)
                return Json(ResponseModel.SuccessResponse("Image Deleted Successfully."));
            return Json(ResponseModel.ErrorResponse("Unable To Delete Image"));
        }

        // Upload image and return saved path
        private async Task<string> UploadImage(IFormFile file)
        {
            string folderPath = Path.Combine(_environment.WebRootPath, "uploads", "room");
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(folderPath, fileName);
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return Path.Combine("uploads", "room", fileName).Replace("\\", "/");
        }
    }
}
