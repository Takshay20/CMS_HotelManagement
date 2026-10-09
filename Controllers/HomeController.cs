using CMS_HotelBooking.Models;
using CMS_HotelBooking.Services.Interfaces;
using CMS_HotelBooking.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMS_HotelBooking.Controllers
{
    public class HomeViewModel
    {
        public List<Slider> Sliders { get; set; } = new();
        public HomeWelcome? Welcome { get; set; }
        public List<HomeWhyChooseUs> WhyChooseUs { get; set; } = new();
        public List<Room> FeaturedRooms { get; set; } = new();
        public List<Feedback> Testimonials { get; set; } = new();
    }

    public class HomeController : Controller
    {
        private readonly ISliderService _sliderService;
        private readonly IHomeWelcomeService _homeWelcomeService;
        private readonly IHomeWhyChooseUsService _whyChooseUsService;
        private readonly IRoomService _roomService;
        private readonly IFeedbackService _feedbackService;
        private readonly IWebHostEnvironment _environment;

        public HomeController(
            ISliderService sliderService,
            IHomeWelcomeService homeWelcomeService,
            IHomeWhyChooseUsService whyChooseUsService,
            IRoomService roomService,
            IFeedbackService feedbackService,
            IWebHostEnvironment environment)
        {
            _sliderService = sliderService;
            _homeWelcomeService = homeWelcomeService;
            _whyChooseUsService = whyChooseUsService;
            _roomService = roomService;
            _feedbackService = feedbackService;
            _environment = environment;
        }

        // Open home page
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var welcome = await _homeWelcomeService.GetAsync();
            var whyChooseUs = await _whyChooseUsService.GetAllAsync();
            var model = new HomeViewModel
            {
                Sliders = await _sliderService.GetAllAsync("Home"),
                Welcome = welcome,
                WhyChooseUs = whyChooseUs
                    .Where(x => x.IsActive)
                    .OrderBy(x => x.DisplayOrder)
                    .ToList(),

                FeaturedRooms = await _roomService.GetFeaturedAsync(6),
                Testimonials = await _feedbackService.GetApprovedAsync(9)
            };
            return View(model);
        }

        // Open privacy page
        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        // Submit feedback
        [HttpPost]
        public async Task<IActionResult> SubmitFeedback(FeedbackVM model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                return Json(
                    ResponseModel.ErrorResponse(
                        "Name is required."
                    )
                );
            }

            if (string.IsNullOrWhiteSpace(model.Message))
            {
                return Json(
                    ResponseModel.ErrorResponse(
                        "Message is required."
                    )
                );
            }

            if (model.Rating < 1 || model.Rating > 5)
            {
                return Json(
                    ResponseModel.ErrorResponse(
                        "Please select a rating between 1 and 5."
                    )
                );
            }

            string? imagePath = null;

            if (model.ImageFile != null)
            {
                string[] allowedExtensions =
                {
                    ".jpg",
                    ".jpeg",
                    ".png",
                    ".webp",
                    ".jfif"
                };

                string extension =
                    Path.GetExtension(
                        model.ImageFile.FileName
                    ).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                {
                    return Json(
                        ResponseModel.ErrorResponse("Only JPG, JPEG, PNG, WEBP and JFIF images are allowed." ));
                }

                if (model.ImageFile.Length > 5 * 1024 * 1024)
                {
                    return Json(ResponseModel.ErrorResponse("Image size cannot exceed 5 MB."));
                }

                string folderPath =Path.Combine(_environment.WebRootPath,"uploads","feedback");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string fileName =Guid.NewGuid().ToString() +extension;

                string filePath =Path.Combine(folderPath, fileName);

                using (FileStream stream =
                    new FileStream(
                        filePath,
                        FileMode.Create
                    ))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                imagePath =
                    Path.Combine(
                        "uploads",
                        "feedback",
                        fileName
                    ).Replace("\\", "/");
            }

            int? userId = null;

            if (User.Identity?.IsAuthenticated == true)
            {
                var claim =
                    User.FindFirst(
                        ClaimTypes.NameIdentifier
                    );

                if (claim != null &&
                    int.TryParse(
                        claim.Value,
                        out int parsedUserId
                    ))
                {
                    userId = parsedUserId;
                }
            }

            var feedback = new Feedback
            {
                UserId = userId,
                Name = model.Name.Trim(),
                Designation = model.Designation?.Trim(),
                ImagePath = imagePath,
                Message = model.Message.Trim(),
                Rating = model.Rating
            };

            var result =
                await _feedbackService.SubmitAsync(
                    feedback
                );

            if (result > 0)
            {
                return Json(
                    ResponseModel.SuccessResponse(
                        "Thank you! Your feedback has been submitted and will appear once approved by our team."
                    )
                );
            }

            return Json(
                ResponseModel.ErrorResponse(
                    "Something went wrong. Please try again."
                )
            );
        }

        // Open error page
        [HttpGet]
        public IActionResult Error()
        {
            return View(
                new ErrorViewModel
                {
                    RequestId =
                        System.Diagnostics.Activity
                            .Current?.Id
                        ?? HttpContext.TraceIdentifier
                }
            );
        }
    }
}