using System;

namespace CMS_HotelBooking.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }

        public int BookingId { get; set; }

        public int? UserId { get; set; }

        public decimal Amount { get; set; }

        public string? OrderId { get; set; }

        public string? GatewayPaymentId { get; set; }

        public string PaymentStatus { get; set; } = "Pending";

        public DateTime? PaymentDate { get; set; }

        public string? FailureReason { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}