using System;

namespace CMS_HotelBooking.Models
{
    public class Users
    {
        public int UserId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string? Phone { get; set; }

        public string? CountryCode { get; set; }

        public string Role { get; set; } = "Customer";

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; }

        public string? ResetCode { get; set; }

        public DateTime? ResetCodeExpiry { get; set; }
    }
}