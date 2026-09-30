using CMS_HotelBooking.Models;
using CMS_HotelBooking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS_HotelBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class FeedbackController : Controller
    {
        private readonly IFeedbackService _service;

        public FeedbackController(IFeedbackService service)
        {
            _service = service;
        }

        // Open feedback page
        public IActionResult Feedback()
        {
            return View();
        }

        // Get all feedback records
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Json(ResponseModel.SuccessResponse("Success", result));
        }

        // Toggle approval
        [HttpPost]
        public async Task<IActionResult> ToggleApproval(int id)
        {
            var result = await _service.ToggleApprovalAsync(id);
            if (result > 0)
                return Json(ResponseModel.SuccessResponse("Status Updated Successfully."));
            return Json(ResponseModel.ErrorResponse("Unable To Update Status"));
        }

        // Delete feedback record
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
