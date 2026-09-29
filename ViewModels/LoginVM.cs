using System.ComponentModel.DataAnnotations;

namespace CMS_HotelBooking.ViewModels
{
    public class LoginVM
    {
        [Required, EmailAddress] public string Email { get; set; } = string.Empty;
        [Required, DataType(DataType.Password)] public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }

        [Required] public string LoginAs { get; set; } = "Customer";
    }
}
