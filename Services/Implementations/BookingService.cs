using CMS_HotelBooking.Helpers;
using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using CMS_HotelBooking.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CMS_HotelBooking.Services.Implementations
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _repository;
        private readonly IRoomRepository _roomRepository;
        private readonly IEmailService _emailService;
        private readonly IPaymentService _paymentService;
        private readonly IServiceScopeFactory _scopeFactory;

        public BookingService(IBookingRepository repository,IRoomRepository roomRepository,IEmailService emailService,IPaymentService paymentService,IServiceScopeFactory scopeFactory)
        {
            _repository = repository;
            _roomRepository = roomRepository;
            _emailService = emailService;
            _paymentService = paymentService;
            _scopeFactory = scopeFactory;
        }
        public async Task<List<Booking>> GetAllAsync() => await _repository.GetAllAsync();

        public async Task<Booking?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task<List<Booking>> GetByUserIdAsync(int userId) => await _repository.GetByUserIdAsync(userId);

        public async Task<(bool Success, string Message)> CreateAsync(Booking model)
        {
            if (model.CheckOutDate <= model.CheckInDate)
                return (false, "Check-out date must be after check-in date.");

            int nights = (model.CheckOutDate - model.CheckInDate).Days;
            if (nights > 60)
                return (false, "Please select a valid date range (maximum 60 nights per booking).");

            var room = await _roomRepository.GetByIdAsync(model.RoomId);
            if (room == null || !room.IsAvailable)
                return (false, "Selected room is not available for booking.");

            var (available, message) = await CheckAvailabilityAsync(model.RoomId, model.CheckInDate, model.CheckOutDate);
            if (!available)
                return (false, message);

            model.TotalPrice = nights * room.PricePerNight;

            var newId = await _repository.CreateAsync(model);
            if (newId <= 0)
                return (false, "Something went wrong while creating your booking.");

            model.BookingId = newId;
            model.RoomTitle = room.Title;
            model.RoomNumber = room.RoomNumber;

            // Send the email in background so the guest does not wait for SMTP
            _ = Task.Run(() => SendBookingReceivedInBackgroundAsync(model));

            return (true, "Your booking request has been submitted. Once our team confirms it, you can pay from the My Bookings page.");
        }

        private async Task SendBookingReceivedInBackgroundAsync(Booking model)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();

                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                var repository = scope.ServiceProvider.GetRequiredService<IBookingRepository>();

                var emailStatus = await emailService.SendBookingReceivedAsync(model);
                await repository.UpdateEmailStatusAsync(model.BookingId, emailStatus, "BookingReceived");
            }
            catch
            {
                // email failure must not affect the booking
            }
        }

        public async Task<(int Result, string EmailNote)> UpdateStatusWithEmailAsync(int id, string status)
        {
            var result = await _repository.UpdateStatusAsync(id, status);
            if (result <= 0)
                return (result, "");

            string emailNote = "";

            if (status == "Approved" || status == "Rejected")
            {
                var booking = await _repository.GetByIdAsync(id);
                if (booking != null)
                {
                    var emailStatus = await _emailService.SendBookingStatusUpdateAsync(booking);
                    await _repository.UpdateEmailStatusAsync(id, emailStatus, "StatusUpdate");
                    emailNote = EmailNoteFor(emailStatus);
                }
            }
            else if (status == "Cancelled")
            {
                var booking = await _repository.GetByIdAsync(id);
                if (booking != null)
                {
                    var emailStatus = await _emailService.SendBookingCancelledAsync(booking);
                    await _repository.UpdateEmailStatusAsync(id, emailStatus, "Cancelled");
                    emailNote = EmailNoteFor(emailStatus);
                }
            }

            return (result, emailNote);
        }

        public async Task<int> UpdateStatusAsync(int id, string status)
        {
            var (result, _) = await UpdateStatusWithEmailAsync(id, status);
            return result;
        }

        public async Task<int> DeleteAsync(int id) => await _repository.DeleteAsync(id);

        public async Task<(bool Available, string Message)> CheckAvailabilityAsync(int roomId, DateTime checkIn, DateTime checkOut, int? excludeBookingId = null)
        {
            if (checkOut <= checkIn)
                return (false, "Check-out date must be after check-in date.");

            var conflicts = await _repository.GetConflictCountAsync(roomId, checkIn, checkOut, excludeBookingId);
            if (conflicts > 0)
                return (false, "This room is already booked for the selected dates. Please choose different dates or another room.");

            return (true, "Room is available for the selected dates.");
        }

        public async Task<List<Room>> GetAlternativeRoomsAsync(int bookingId)
        {
            var booking = await _repository.GetByIdAsync(bookingId);
            if (booking == null) return new List<Room>();

            var candidates = await _repository.GetRoomsByCategoryAsync(booking.RoomCategoryId, booking.RoomId);
            var freeRooms = new List<Room>();

            foreach (var room in candidates)
            {
                var conflicts = await _repository.GetConflictCountAsync(room.RoomId, booking.CheckInDate, booking.CheckOutDate, bookingId);
                if (conflicts == 0)
                    freeRooms.Add(room);
            }

            return freeRooms;
        }

        public async Task<(bool Success, string Message)> ProposeRoomChangeAsync(
    int bookingId,
    int proposedRoomId,
    string? note)
        {
            var booking =
                await _repository.GetByIdAsync(bookingId);

            if (booking == null)
            {
                return (false, "Booking not found.");
            }

            var proposedRoom =
                await _roomRepository.GetByIdAsync(proposedRoomId);

            if (proposedRoom == null)
            {
                return (false, "Selected room not found.");
            }

            if (proposedRoom.RoomCategoryId != booking.RoomCategoryId)
            {
                return (
                    false,
                    "Please choose a room from the same category as the original booking."
                );
            }

            var conflicts =
                await _repository.GetConflictCountAsync(
                    proposedRoomId,
                    booking.CheckInDate,
                    booking.CheckOutDate,
                    bookingId
                );

            if (conflicts > 0)
            {
                return (
                    false,
                    "The selected room is also booked for these dates. Please choose a different room."
                );
            }

            var result =
                await _repository.ProposeRoomChangeAsync(
                    bookingId,
                    proposedRoomId,
                    note
                );

            if (result <= 0)
            {
                return (
                    false,
                    "Unable to propose room change."
                );
            }

            return (
                true,
                "Room change proposed successfully."
            );
        }
        public async Task<(bool Success, string Message)> SendRoomChangeEmailAsync(
    int bookingId)
        {
            var booking =
                await _repository.GetByIdAsync(bookingId);

            if (booking == null)
            {
                return (false, "Booking not found.");
            }

            var emailStatus =
                await _emailService.SendRoomChangeRequestAsync(
                    booking
                );

            await _repository.UpdateEmailStatusAsync(
                bookingId,
                emailStatus,
                "RoomChange"
            );

            return (
                true,
                EmailNoteFor(emailStatus)
            );
        }

        public async Task<(bool Success, string Message)> RespondRoomChangeAsync(
    int bookingId,
    int userId,
    bool accept)
        {
            var booking =
                await _repository.GetByIdAsync(bookingId);

            if (booking == null || booking.UserId != userId)
            {
                return (false, "Booking not found.");
            }

            if (booking.RoomChangeStatus != "Pending")
            {
                return (
                    false,
                    "There is no pending room change request for this booking."
                );
            }

            var result =
                await _repository.RespondRoomChangeAsync(
                    bookingId,
                    accept
                );

            if (result <= 0)
            {
                return (
                    false,
                    "Unable to process your response. Please try again."
                );
            }

            if (!accept)
            {
                var emailStatus =
                    await _emailService.SendBookingCancelledAsync(
                        booking
                    );

                await _repository.UpdateEmailStatusAsync(
                    bookingId,
                    emailStatus,
                    "Cancelled"
                );

                return (
                    true,
                    "Your booking has been cancelled as requested."
                );
            }

            var updatedBooking =
                await _repository.GetByIdAsync(bookingId);

            if (updatedBooking == null)
            {
                return (
                    false,
                    "Booking was updated but could not be loaded again."
                );
            }

            var payment =
                await _paymentService.GetByBookingIdAsync(
                    bookingId
                );

            if (payment != null)
            {
                var paymentResult =
                    await _paymentService.UpdatePaymentAmountAsync(
                        payment.PaymentId,
                        updatedBooking.TotalPrice
                    );

                if (!paymentResult.Success)
                {
                    return (
                        false,
                        "Room changed successfully, but payment amount could not be updated."
                    );
                }
            }

            return (
                true,
                "Great! Your booking and payment amount have been updated to the new room."
            );
        }

        public async Task<(bool Success, string Message, int RefundPercentage, decimal RefundAmount)> CancelByGuestAsync(int bookingId, int userId)
        {
            var booking = await _repository.GetByIdAsync(bookingId);
            if (booking == null || booking.UserId != userId)
                return (false, "Booking not found.", 0, 0);

            if (booking.Status == "Cancelled")
                return (false, "This booking is already cancelled.", 0, 0);

            if (booking.Status == "Rejected")
                return (false, "This booking was already rejected and cannot be cancelled.", 0, 0);

            if (booking.CheckInDate.Date <= DateTime.Today)
                return (false, "Check-in date has arrived, so this booking can no longer be cancelled online. Please contact the hotel.", 0, 0);

            var (percentage, amount) = RefundPolicyHelper.Calculate(booking.CheckInDate, booking.TotalPrice);

            var result = await _repository.CancelWithRefundAsync(bookingId, percentage, amount, "Guest");
            if (result <= 0)
                return (false, "Unable to cancel booking. Please try again.", 0, 0);

            booking.Status = "Cancelled";
            var emailStatus = await _emailService.SendBookingCancelledAsync(booking);
            await _repository.UpdateEmailStatusAsync(bookingId, emailStatus, "Cancelled");

            var message = "Your booking has been cancelled. " + RefundPolicyHelper.PolicyText(percentage);
            return (true, message, percentage, amount);
        }

        private static string EmailNoteFor(string emailStatus) => emailStatus switch
        {
            "Sent" => " Confirmation email sent to the guest.",
            "Failed" => " (Note: the notification email failed to send - check SMTP settings in Site Setting.)",
            "Disabled" => " (Note: email notifications are currently turned off.)",
            _ => ""
        };
    }
}
