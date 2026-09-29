using CMS_HotelBooking.Models;
using CMS_HotelBooking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS_HotelBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ContactMessageController : Controller
    {
        private readonly IContactMessageService _service;

        public ContactMessageController(IContactMessageService service)
        {
            _service = service;
        }

        public IActionResult ContactMessage()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Json(ResponseModel.SuccessResponse("Success", result));
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return Json(ResponseModel.SuccessResponse("Success", result));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (result > 0)
                return Json(ResponseModel.SuccessResponse("Record Deleted Successfully."));
            return Json(ResponseModel.ErrorResponse("Unable To Delete Record"));
        }

        [HttpPost]
        public async Task<IActionResult> Reply(int id, string adminReply)
        {
            if (string.IsNullOrWhiteSpace(adminReply))
                return Json(ResponseModel.ErrorResponse("Please write a reply message."));

            var result = await _service.ReplyAsync(id, adminReply);
            if (result > 0)
                return Json(ResponseModel.SuccessResponse("Reply sent successfully."));
            return Json(ResponseModel.ErrorResponse("Unable To Send Reply"));
        }
    }
}
