using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using Dapper;
using System.Data;

namespace CMS_HotelBooking.Repository.Implementations
{
    public class BookingReminderRepository : BaseRepository, IBookingReminderRepository
    {
        public BookingReminderRepository(IConfiguration configuration)
            : base(configuration)
        {
        }

        // Get by booking and type
        public async Task<BookingReminder?> GetByBookingAndTypeAsync(
            int bookingId,
            string reminderType)
        {
            using var connection = GetConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@BookingId", bookingId);
            parameters.Add("@ReminderType", reminderType);

            return await connection.QueryFirstOrDefaultAsync<BookingReminder>(
                "sp_GetBookingReminder",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        // Create booking reminder record
        public async Task<int> CreateAsync(
            BookingReminder reminder)
        {
            using var connection = GetConnection();

            var parameters = new DynamicParameters();

            parameters.Add(
                "@BookingId",
                reminder.BookingId
            );

            parameters.Add(
                "@ReminderType",
                reminder.ReminderType
            );

            parameters.Add(
                "@SentDate",
                reminder.SentDate
            );

            parameters.Add(
                "@IsSent",
                reminder.IsSent
            );

            return await connection.QueryFirstOrDefaultAsync<int>(
                "sp_CreateBookingReminder",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        // Get bookings for reminder
        public async Task<List<Booking>> GetBookingsForReminderAsync()
        {
            using var connection = GetConnection();

            var result = await connection.QueryAsync<Booking>(
                "sp_GetBookingsForReminder",
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }
    }
}