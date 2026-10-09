using System.ComponentModel.DataAnnotations;

namespace CMS_HotelBooking.ViewModels
{
    public class RegisterVM : IValidatableObject
    {
        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Full Name must be between 3 and 150 characters.")]
        public string FullName { get; set; } = string.Empty;


        [Required(ErrorMessage = "Email address is required.")]
        [RegularExpression( @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9-]+(\.[A-Za-z0-9-]+)+$",ErrorMessage = "Please enter a valid email address.")]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Country code is required.")]
        public string CountryCode { get; set; } = "+91";

        [Required(ErrorMessage = "Phone number is required.")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm Password is required.")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            string code = CountryCode?.Trim() ?? "";
            string phone = Phone?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(phone))
                yield break;

            if (!phone.All(char.IsDigit))
            {
                yield return new ValidationResult(
                    "Please enter a valid phone number.",
                    new[] { nameof(Phone) }
                );

                yield break;
            }

            int expectedLength = code switch
            {
                "+91" => 10,
                "+1" => 10,
                "+44" => 10,
                "+61" => 9,
                "+971" => 9,
                "+65" => 8,
                "+81" => 10,
                "+49" => 10,
                "+33" => 9,
                _ => 10
            };

            if (phone.Length != expectedLength)
            {
                yield return new ValidationResult(
                    "Please enter a valid phone number.",
                    new[] { nameof(Phone) }
                );
            }
        }
    }
}