using System;
using System.ComponentModel.DataAnnotations;

namespace CMS_HotelBooking.ViewModels
{
    public class BookingVM
    {
        [Required] public int RoomId { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Full name must be between 3 and 150 characters.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^[0-9+\-\s]{7,15}$", ErrorMessage = "Please enter a valid phone number.")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Check-in date is required.")]
        public DateTime CheckInDate { get; set; }

        [Required(ErrorMessage = "Check-out date is required.")]
        public DateTime CheckOutDate { get; set; }

        [Range(1, 20, ErrorMessage = "Guests must be between 1 and 20.")]
        public int Guests { get; set; } = 1;

        [StringLength(500, ErrorMessage = "Special request cannot exceed 500 characters.")]
        public string? SpecialRequest { get; set; }
    }
}
