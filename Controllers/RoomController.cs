using CMS_HotelBooking.Models;
using CMS_HotelBooking.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMS_HotelBooking.Controllers
{
    public class RoomListViewModel
    {
        public List<Slider> Sliders { get; set; } = new();
        public List<Room> Rooms { get; set; } = new();
        public List<RoomCategory> Categories { get; set; } = new();
    }

    public class RoomController : Controller
    {
        private readonly ISliderService _sliderService;
        private readonly IRoomService _roomService;
        private readonly IRoomCategoryService _categoryService;
        private readonly IAmenityService _amenityService;
        private readonly IUsersService _usersService;

        public RoomController(
            ISliderService sliderService,
            IRoomService roomService,
            IRoomCategoryService categoryService,
            IAmenityService amenityService,
            IUsersService usersService)
        {
            _sliderService = sliderService;
            _roomService = roomService;
            _categoryService = categoryService;
            _amenityService = amenityService;
            _usersService = usersService;
        }

        // Open room page
        public async Task<IActionResult> Index()
        {
            var model = new RoomListViewModel
            {
                Sliders = await _sliderService.GetAllAsync("Room"),
                Rooms = (await _roomService.GetAllAsync()).Where(r => r.IsAvailable).ToList(),
                Categories = await _categoryService.GetAllAsync()
            };
            return View(model);
        }

        // Show room details
        public async Task<IActionResult> Details(int id)
        {
            var room = await _roomService.GetByIdAsync(id);
            if (room == null) return NotFound();

            var allAmenities = await _amenityService.GetAllAsync();
            if (!string.IsNullOrWhiteSpace(room.AmenityIds))
            {
                var ids = room.AmenityIds.Split(',').Select(x => int.TryParse(x, out var v) ? v : 0).ToHashSet();
                room.AmenityList = allAmenities.Where(a => ids.Contains(a.AmenityId)).ToList();
            }

            if (User.Identity?.IsAuthenticated == true)
            {
                var claim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (claim != null && int.TryParse(claim.Value, out var userId))
                {
                    var currentUser = await _usersService.GetByIdAsync(userId);
                    if (currentUser != null)
                    {
                        ViewBag.PrefillName = currentUser.FullName;
                        ViewBag.PrefillEmail = currentUser.Email;
                        ViewBag.PrefillPhone = currentUser.Phone;
                    }
                }
            }

            return View(room);
        }
    }
}
