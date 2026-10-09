using CMS_HotelBooking.Models;
using CMS_HotelBooking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS_HotelBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUsersService _service;

        public UserController(IUsersService service)
        {
            _service = service;
        }

        // Open user page
        public IActionResult User()
        {
            return View();
        }

        // Get all user records
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Json(ResponseModel.SuccessResponse("Success", result));
        }

        // Toggle active
        [HttpPost]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var result = await _service.ToggleActiveAsync(id);
            if (result > 0)
            return Json(ResponseModel.SuccessResponse("Status Updated Successfully."));
            return Json(ResponseModel.ErrorResponse("Unable To Update Status"));
        }

        // Delete user record
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (result > 0)
            return Json(ResponseModel.SuccessResponse("Record Deleted Successfully."));
            return Json(ResponseModel.ErrorResponse("Unable To Delete Record"));
        }
    }
}
