using System.ComponentModel.DataAnnotations;

namespace CMS_HotelBooking.ViewModels
{
    public class ContactVM
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 150 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        [RegularExpression(@"^[0-9+\-\s]{7,15}$", ErrorMessage = "Please enter a valid phone number.")]
        public string? Phone { get; set; }
        public string? Subject { get; set; }
        public string? Message { get; set; }
    }
}
