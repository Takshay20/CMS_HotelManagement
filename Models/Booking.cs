using System;

namespace CMS_HotelBooking.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int? UserId { get; set; }
        public int RoomId { get; set; }

        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }

        public int Guests { get; set; } = 1;

        public string? SpecialRequest { get; set; }

        public decimal TotalPrice { get; set; }

        public string Status { get; set; } = "Pending";

        public DateTime CreatedDate { get; set; }

        public int? ProposedRoomId { get; set; }
        public string? RoomChangeNote { get; set; }
        public string? RoomChangeStatus { get; set; }

        public string? RoomTitle { get; set; }
        public string? RoomNumber { get; set; }
        public int RoomCategoryId { get; set; }

        public string? ProposedRoomTitle { get; set; }
        public string? ProposedRoomNumber { get; set; }

        public string? EmailStatus { get; set; }
        public DateTime? LastEmailSentDate { get; set; }
        public string? LastEmailType { get; set; }

        public int? RefundPercentage { get; set; }
        public decimal? RefundAmount { get; set; }
        public DateTime? CancelledDate { get; set; }
        public string? CancelledBy { get; set; }

        public int? PaymentId { get; set; }
        public string? PaymentStatus { get; set; }
        public string? OrderId { get; set; }
        public string? GatewayPaymentId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? FailureReason { get; set; }
    }
}