using CMS_HotelBooking.Models;
using CMS_HotelBooking.Services.Interfaces;
using System.Net;
using System.Net.Mail;

namespace CMS_HotelBooking.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Send email
        private async Task<string> SendEmailAsync(
            string toEmail,
            string toName,
            string subject,
            string body)
        {
            try
            {
                bool isEnabled = _configuration.GetValue<bool>(
                    "EmailSettings:IsEnabled",
                    false
                );

                if (!isEnabled)
                    return "Disabled";

                string smtpHost =
                    _configuration["EmailSettings:SmtpHost"]
                    ?? "smtp.gmail.com";

                int smtpPort =
                    _configuration.GetValue<int>(
                        "EmailSettings:SmtpPort",
                        587
                    );

                bool enableSsl =
                    _configuration.GetValue<bool>(
                        "EmailSettings:EnableSsl",
                        true
                    );

                string senderEmail =
                    _configuration["EmailSettings:SenderEmail"]
                    ?? "";

                string senderPassword =
                    _configuration["EmailSettings:SenderPassword"]
                    ?? "";

                string senderName =
                    _configuration["EmailSettings:SenderName"]
                    ?? "Royal Paradise Hotel";

                if (string.IsNullOrWhiteSpace(toEmail))
                    return "Failed";

                if (string.IsNullOrWhiteSpace(senderEmail))
                    return "Failed";

                if (string.IsNullOrWhiteSpace(senderPassword))
                    return "Failed";

                using var mail = new MailMessage();

                mail.From = new MailAddress(
                    senderEmail,
                    senderName
                );

                mail.To.Add(
                    new MailAddress(
                        toEmail,
                        string.IsNullOrWhiteSpace(toName)
                            ? "Guest"
                            : toName
                    )
                );

                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.SubjectEncoding = System.Text.Encoding.UTF8;

                using var smtp = new SmtpClient(
                    smtpHost,
                    smtpPort
                );

                smtp.EnableSsl = enableSsl;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(
                    senderEmail,
                    senderPassword
                );

                await smtp.SendMailAsync(mail);

                return "Sent";
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "Email Error: " + ex.Message
                );

                return "Failed";
            }
        }

        // Send booking received
        public async Task<string> SendBookingReceivedAsync(
            Booking booking)
        {
            string room = GetRoomName(
                booking.RoomTitle,
                booking.RoomNumber
            );

            string body = BuildEmailLayout(
                "Booking Request Received",
                "#d4af37",
                $@"
                    <h2 style='margin-top:0;color:#222;'>
                        Thank You, {booking.FullName}
                    </h2>

                    <p style='color:#555;line-height:1.7;'>
                        We have successfully received your booking
                        request at <strong>Royal Paradise Hotel</strong>.
                    </p>

                    <div style='background:#fff8df;
                                border:1px solid #ead38a;
                                padding:15px;
                                border-radius:8px;
                                margin:25px 0;
                                text-align:center;'>

                        <strong style='color:#a57b00;font-size:17px;'>
                            BOOKING REQUEST RECEIVED
                        </strong>

                    </div>

                    {BuildBookingDetails(booking, room)}

                    <p style='color:#666;line-height:1.7;margin-top:25px;'>
                        Your booking is currently under review.
                        We will send you another email once the hotel
                        administrator approves or rejects your booking.
                    </p>
                "
            );

            return await SendEmailAsync(
                booking.Email,
                booking.FullName,
                $"Booking Request Received - #{booking.BookingId}",
                body
            );
        }

        // Send booking status update
        public async Task<string> SendBookingStatusUpdateAsync(
            Booking booking)
        {
            string status =
                string.IsNullOrWhiteSpace(booking.Status)
                    ? "Pending"
                    : booking.Status;

            string heading = "Booking Status Updated";
            string color = "#d4af37";
            string message = "Your booking status has been updated.";

            if (status.Equals("Approved", StringComparison.OrdinalIgnoreCase))
            {
                heading = "Booking Approved";
                color = "#168a45";
                message =
                    "Great news! Your booking has been approved by our hotel administrator.";
            }
            else if (status.Equals("Rejected", StringComparison.OrdinalIgnoreCase))
            {
                heading = "Booking Rejected";
                color = "#c0392b";
                message =
                    "We are sorry to inform you that your booking request has been rejected.";
            }
            else if (status.Equals("Cancelled", StringComparison.OrdinalIgnoreCase))
            {
                heading = "Booking Cancelled";
                color = "#c0392b";
                message = "Your booking has been cancelled.";
            }

            string paymentMessage = "";

            if (status.Equals("Approved", StringComparison.OrdinalIgnoreCase))
            {
                string paymentStatus =
                    string.IsNullOrWhiteSpace(booking.PaymentStatus)
                        ? "Pending"
                        : booking.PaymentStatus;

                if (paymentStatus.Equals("Paid", StringComparison.OrdinalIgnoreCase))
                {
                    paymentMessage = @"
                        <div style='background:#eef9f1;
                                    border:1px solid #b8dfc3;
                                    padding:20px;
                                    border-radius:8px;
                                    margin-top:25px;'>

                            <h3 style='margin:0 0 10px;color:#287a45;'>
                                Payment Completed
                            </h3>

                            <p style='margin:0;color:#555;line-height:1.7;'>
                                Your payment has been
                                <strong>successfully completed</strong>.
                                <br /><br />
                                Your booking is fully confirmed.
                            </p>

                        </div>";
                }
                else
                {
                    paymentMessage = @"
                        <div style='background:#fff8e6;
                                    border:1px solid #ead38a;
                                    padding:20px;
                                    border-radius:8px;
                                    margin-top:25px;'>

                            <h3 style='margin:0 0 10px;color:#9a6b00;'>
                                Payment Pending
                            </h3>

                            <p style='margin:0;color:#555;line-height:1.7;'>
                                Your booking has been approved,
                                but your
                                <strong>payment is still pending.</strong>

                                <br /><br />

                                Please login to the
                                <strong>Royal Paradise Hotel</strong>
                                website and open
                                <strong>My Bookings</strong>.

                                <br /><br />

                                Click the
                                <strong>Pay Now</strong>
                                button to complete your payment securely.
                            </p>

                        </div>";
                }
            }

            string room = GetRoomName(
                booking.RoomTitle,
                booking.RoomNumber
            );

            string body = BuildEmailLayout(
                heading,
                color,
                $@"
                    <h2 style='margin-top:0;color:#222;'>
                        Dear {booking.FullName},
                    </h2>

                    <p style='color:#555;line-height:1.7;'>
                        {message}
                    </p>

                    <div style='background:{color}15;
                                border:1px solid {color};
                                padding:16px;
                                border-radius:8px;
                                margin:25px 0;
                                text-align:center;'>

                        <strong style='color:{color};font-size:18px;'>
                            {status.ToUpper()}
                        </strong>

                    </div>

                    {BuildBookingDetails(booking, room)}

                    {paymentMessage}
                "
            );

            return await SendEmailAsync(
                booking.Email,
                booking.FullName,
                $"Booking {status} - #{booking.BookingId}",
                body
            );
        }

        // Send payment completed
        public async Task<string> SendPaymentCompletedAsync(
            Booking booking,
            Payment payment)
        {
            string room = GetRoomName(
                booking.RoomTitle,
                booking.RoomNumber
            );

            string body = BuildEmailLayout(
                "Payment Completed",
                "#198754",
                $@"
                    <h2 style='margin-top:0;color:#222;'>
                        Dear {booking.FullName},
                    </h2>

                    <div style='background:#eef9f1;
                                border:1px solid #b8dfc3;
                                padding:20px;
                                border-radius:8px;
                                margin:20px 0;
                                text-align:center;'>

                        <div style='font-size:42px;color:#198754;'>
                            ✓
                        </div>

                        <h2 style='color:#198754;margin:10px 0;'>
                            Payment Successful
                        </h2>

                        <p style='color:#555;line-height:1.7;margin:0;'>
                            Your payment has been successfully completed.
                            Your booking is now fully confirmed.
                        </p>

                    </div>

                    <div style='background:#f7f7f7;
                                border-radius:8px;
                                padding:18px;
                                margin:20px 0;'>

                        <table style='width:100%;border-collapse:collapse;'>

                            <tr>
                                <td style='padding:8px;color:#777;'>
                                    Payment ID
                                </td>
                                <td style='padding:8px;text-align:right;font-weight:bold;'>
                                    #{payment.PaymentId}
                                </td>
                            </tr>

                            <tr>
                                <td style='padding:8px;color:#777;'>
                                    Amount Paid
                                </td>
                                <td style='padding:8px;text-align:right;font-weight:bold;color:#198754;'>
                                    ₹ {payment.Amount:N2}
                                </td>
                            </tr>

                            <tr>
                                <td style='padding:8px;color:#777;'>
                                    Order ID
                                </td>
                                <td style='padding:8px;text-align:right;font-weight:bold;'>
                                    {payment.OrderId}
                                </td>
                            </tr>

                            <tr>
                                <td style='padding:8px;color:#777;'>
                                    Payment ID
                                </td>
                                <td style='padding:8px;text-align:right;font-weight:bold;'>
                                    {payment.GatewayPaymentId}
                                </td>
                            </tr>

                            <tr>
                                <td style='padding:8px;color:#777;'>
                                    Payment Date
                                </td>
                                <td style='padding:8px;text-align:right;font-weight:bold;'>
                                    {(payment.PaymentDate ?? DateTime.Now):dd MMM yyyy hh:mm tt}
                                </td>
                            </tr>

                        </table>

                    </div>

                    {BuildBookingDetails(booking, room)}

                    <div style='background:#eef9f1;
                                border:1px solid #b8dfc3;
                                padding:18px;
                                border-radius:8px;
                                margin-top:25px;'>

                        <p style='margin:0;color:#287a45;line-height:1.7;'>
                            Thank you for choosing
                            <strong>Royal Paradise Hotel</strong>.
                            We look forward to welcoming you.
                        </p>

                    </div>
                "
            );

            return await SendEmailAsync(
                booking.Email,
                booking.FullName,
                $"Payment Successful - Booking #{booking.BookingId}",
                body
            );
        }

        // Send room change request
        public async Task<string> SendRoomChangeRequestAsync(
            Booking booking)
        {
            string oldRoom = GetRoomName(
                booking.RoomTitle,
                booking.RoomNumber
            );

            string newRoom = GetRoomName(
                booking.ProposedRoomTitle,
                booking.ProposedRoomNumber
            );

            string note =
                string.IsNullOrWhiteSpace(booking.RoomChangeNote)
                    ? "The hotel has proposed an alternative room for your booking."
                    : booking.RoomChangeNote;

            string body = BuildEmailLayout(
                "Room Change Request",
                "#d4af37",
                $@"
                    <h2 style='margin-top:0;color:#222;'>
                        Dear {booking.FullName},
                    </h2>

                    <p style='color:#555;line-height:1.7;'>
                        We need to make a small change to your room reservation.
                    </p>

                    <div style='background:#fff8df;
                                border:1px solid #ead38a;
                                padding:20px;
                                border-radius:8px;
                                margin:25px 0;'>

                        <p style='margin:0 0 8px;color:#777;'>
                            Current Room
                        </p>

                        <strong style='font-size:17px;color:#222;'>
                            {oldRoom}
                        </strong>

                        <hr style='border:0;border-top:1px solid #e4d9b2;margin:18px 0;' />

                        <p style='margin:0 0 8px;color:#777;'>
                            Proposed Room
                        </p>

                        <strong style='font-size:17px;color:#a57b00;'>
                            {newRoom}
                        </strong>

                    </div>

                    <p style='color:#555;line-height:1.7;'>
                        <strong>Reason / Note:</strong>
                        <br />
                        {note}
                    </p>

                    {BuildBookingDates(booking)}

                    <div style='background:#eef4ff;
                                border:1px solid #c7d7f5;
                                padding:18px;
                                border-radius:8px;
                                margin-top:25px;'>

                        <p style='margin:0;color:#345;line-height:1.7;'>
                            Please login to the
                            <strong>Royal Paradise Hotel</strong>
                            website and open
                            <strong>My Bookings</strong>.
                        </p>

                        <p style='margin:12px 0 0;color:#345;line-height:1.7;'>
                            There you can review the proposed room and
                            <strong>Accept</strong> or
                            <strong>Reject</strong> the room change request.
                        </p>

                    </div>
                "
            );

            return await SendEmailAsync(
                booking.Email,
                booking.FullName,
                $"Room Change Request - Booking #{booking.BookingId}",
                body
            );
        }

        // Send booking cancelled
        public async Task<string> SendBookingCancelledAsync(
            Booking booking)
        {
            string room = GetRoomName(
                booking.RoomTitle,
                booking.RoomNumber
            );

            string body = BuildEmailLayout(
                "Booking Cancelled",
                "#c0392b",
                $@"
                    <h2 style='margin-top:0;color:#222;'>
                        Dear {booking.FullName},
                    </h2>

                    <p style='color:#555;line-height:1.7;'>
                        Your booking with
                        <strong>Royal Paradise Hotel</strong>
                        has been successfully cancelled.
                    </p>

                    <div style='background:#fff0ef;
                                border:1px solid #e5aaa5;
                                padding:16px;
                                border-radius:8px;
                                margin:25px 0;
                                text-align:center;'>

                        <strong style='color:#c0392b;font-size:18px;'>
                            BOOKING CANCELLED
                        </strong>

                    </div>

                    {BuildBookingDetails(booking, room)}
                "
            );

            return await SendEmailAsync(
                booking.Email,
                booking.FullName,
                $"Booking Cancelled - #{booking.BookingId}",
                body
            );
        }

        // Send password reset code
        public async Task<string> SendPasswordResetCodeAsync(
            string toEmail,
            string fullName,
            string resetCode)
        {
            string name =
                string.IsNullOrWhiteSpace(fullName)
                    ? "Guest"
                    : fullName;

            string body = BuildEmailLayout(
                "Password Reset Request",
                "#d4af37",
                $@"
                    <h2 style='margin-top:0;color:#222;'>
                        Hi {name},
                    </h2>

                    <p style='color:#555;line-height:1.7;'>
                        We received a request to reset the password
                        for your Royal Paradise Hotel account.
                    </p>

                    <div style='background:#fff8df;
                                border:1px solid #ead38a;
                                padding:22px;
                                border-radius:8px;
                                margin:25px 0;
                                text-align:center;'>

                        <span style='color:#a57b00;
                                     font-size:32px;
                                     font-weight:bold;
                                     letter-spacing:10px;'>
                            {resetCode}
                        </span>

                    </div>

                    <p style='color:#666;line-height:1.7;'>
                        This code is valid for
                        <strong>10 minutes</strong>.
                        If you did not request a password reset,
                        you can safely ignore this email.
                    </p>
                "
            );

            return await SendEmailAsync(
                toEmail,
                name,
                "Your Password Reset Code - Royal Paradise Hotel",
                body
            );
        }

        // Send booking reminder
        public async Task<string> SendBookingReminderAsync(
            Booking booking,
            string reminderType)
        {
            string subject;
            string heading;
            string message;

            switch (reminderType)
            {
                case "24H":
                    subject =
                        $"Reminder: Your Stay is Tomorrow - Booking #{booking.BookingId}";
                    heading = "Your Stay is Tomorrow";
                    message =
                        "This is a friendly reminder that your stay at Royal Paradise Hotel is scheduled for tomorrow.";
                    break;

                case "12H":
                    subject =
                        $"Reminder: Your Stay is in 12 Hours - Booking #{booking.BookingId}";
                    heading = "Your Stay is in 12 Hours";
                    message =
                        "This is a friendly reminder that your stay at Royal Paradise Hotel is scheduled in approximately 12 hours.";
                    break;

                case "6H":
                    subject =
                        $"Reminder: Your Stay Starts in 6 Hours - Booking #{booking.BookingId}";
                    heading = "Your Stay Starts in 6 Hours";
                    message =
                        "Your stay at Royal Paradise Hotel is scheduled to begin in approximately 6 hours.";
                    break;

                default:
                    subject =
                        $"Booking Reminder - #{booking.BookingId}";
                    heading = "Booking Reminder";
                    message =
                        "This is a reminder about your upcoming stay at Royal Paradise Hotel.";
                    break;
            }

            string room = GetRoomName(
                booking.RoomTitle,
                booking.RoomNumber
            );

            string body = BuildEmailLayout(
                heading,
                "#d4af37",
                $@"
                    <h2 style='margin-top:0;color:#222;'>
                        Dear {booking.FullName},
                    </h2>

                    <p style='color:#555;line-height:1.7;'>
                        {message}
                    </p>

                    <div style='background:#fff8df;
                                border:1px solid #ead38a;
                                padding:18px;
                                border-radius:8px;
                                margin:25px 0;
                                text-align:center;'>

                        <strong style='color:#a57b00;font-size:18px;'>
                            BOOKING REMINDER
                        </strong>

                    </div>

                    {BuildBookingDetails(booking, room)}

                    <div style='background:#eef9f1;
                                border:1px solid #b8dfc3;
                                padding:18px;
                                border-radius:8px;
                                margin-top:25px;'>

                        <p style='margin:0;color:#287a45;line-height:1.7;'>
                            <strong>Check-in Time:</strong> 11:00 AM
                            <br />
                            <strong>Check-out Time:</strong> 12:00 PM
                        </p>

                    </div>

                    <p style='color:#666;line-height:1.7;margin-top:25px;'>
                        Please arrive at the hotel on time for a smooth
                        check-in experience.
                    </p>
                "
            );

            return await SendEmailAsync(
                booking.Email,
                booking.FullName,
                subject,
                body
            );
        }

        // Get room name
        private string GetRoomName(
            string? title,
            string? number)
        {
            string room =
                string.IsNullOrWhiteSpace(title)
                    ? "Hotel Room"
                    : title;

            if (!string.IsNullOrWhiteSpace(number))
                room += $" - Room {number}";

            return room;
        }

        // Build booking details
        private string BuildBookingDetails(
            Booking booking,
            string room)
        {
            return $@"
                <h3 style='color:#222;
                           border-bottom:1px solid #ddd;
                           padding-bottom:10px;'>
                    Booking Details
                </h3>

                <table style='width:100%;border-collapse:collapse;'>

                    <tr>
                        <td style='padding:10px;color:#777;border-bottom:1px solid #eee;'>
                            Booking ID
                        </td>

                        <td style='padding:10px;text-align:right;font-weight:bold;border-bottom:1px solid #eee;'>
                            #{booking.BookingId}
                        </td>
                    </tr>

                    <tr>
                        <td style='padding:10px;color:#777;border-bottom:1px solid #eee;'>
                            Room
                        </td>

                        <td style='padding:10px;text-align:right;font-weight:bold;border-bottom:1px solid #eee;'>
                            {room}
                        </td>
                    </tr>

                    <tr>
                        <td style='padding:10px;color:#777;border-bottom:1px solid #eee;'>
                            Check-in
                        </td>

                        <td style='padding:10px;text-align:right;font-weight:bold;border-bottom:1px solid #eee;'>
                            {booking.CheckInDate:dd MMM yyyy}
                        </td>
                    </tr>

                    <tr>
                        <td style='padding:10px;color:#777;border-bottom:1px solid #eee;'>
                            Check-out
                        </td>

                        <td style='padding:10px;text-align:right;font-weight:bold;border-bottom:1px solid #eee;'>
                            {booking.CheckOutDate:dd MMM yyyy}
                        </td>
                    </tr>

                    <tr>
                        <td style='padding:10px;color:#777;border-bottom:1px solid #eee;'>
                            Guests
                        </td>

                        <td style='padding:10px;text-align:right;font-weight:bold;border-bottom:1px solid #eee;'>
                            {booking.Guests}
                        </td>
                    </tr>

                    <tr>
                        <td style='padding:12px 10px;color:#222;font-weight:bold;'>
                            Total Amount
                        </td>

                        <td style='padding:12px 10px;text-align:right;color:#d4af37;font-size:18px;font-weight:bold;'>
                            ₹ {booking.TotalPrice:N2}
                        </td>
                    </tr>

                </table>
            ";
        }

        // Build booking dates
        private string BuildBookingDates(
            Booking booking)
        {
            return $@"
                <div style='margin-top:25px;
                            padding:15px;
                            background:#f7f7f7;
                            border-radius:8px;'>

                    <strong>Check-in:</strong>
                    {booking.CheckInDate:dd MMM yyyy}

                    &nbsp;&nbsp; | &nbsp;&nbsp;

                    <strong>Check-out:</strong>
                    {booking.CheckOutDate:dd MMM yyyy}

                </div>
            ";
        }

        // Build common email layout
        private string BuildEmailLayout(
            string title,
            string color,
            string content)
        {
            string hotelEmail =
                _configuration["EmailSettings:SenderEmail"]
                ?? "";

            string hotelName =
                _configuration["EmailSettings:SenderName"]
                ?? "Royal Paradise Hotel";

            return $@"
<!DOCTYPE html>
<html>

<head>
    <meta charset='UTF-8' />
    <meta name='viewport'
          content='width=device-width,initial-scale=1.0' />
</head>

<body style='margin:0;
             padding:0;
             background:#f4f4f4;
             font-family:Arial,Helvetica,sans-serif;'>

    <div style='max-width:650px;
                margin:30px auto;
                background:#ffffff;
                border-radius:10px;
                overflow:hidden;
                box-shadow:0 5px 25px rgba(0,0,0,.10);'>

        <div style='background:#111a2e;
                    padding:30px;
                    text-align:center;
                    border-bottom:4px solid #d4af37;'>

            <h1 style='margin:0;
                       color:#d4af37;
                       font-family:Georgia,serif;
                       font-size:28px;
                       font-weight:500;'>

                {hotelName}

            </h1>

            <p style='margin:8px 0 0;
                      color:#ffffff;
                      font-size:12px;
                      letter-spacing:3px;'>

                LUXURY • ELEGANCE • EXCELLENCE

            </p>

        </div>

        <div style='padding:22px 35px 0;'>

            <h3 style='margin:0;
                       color:{color};
                       font-size:13px;
                       letter-spacing:2px;
                       text-transform:uppercase;'>

                {title}

            </h3>

        </div>

        <div style='padding:20px 35px 35px;'>

            {content}

        </div>

        <div style='background:#f7f7f7;
                    padding:20px;
                    text-align:center;
                    color:#888;
                    font-size:12px;'>

            <p style='margin:0 0 6px;'>
                Thank you for choosing
                <strong>{hotelName}</strong>.
            </p>

            <p style='margin:0;'>
                {hotelEmail}
            </p>

        </div>

    </div>

</body>

</html>
";
        }
    }
}