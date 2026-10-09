using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using Dapper;
using System.Data;

namespace CMS_HotelBooking.Repository.Implementations
{
    public class BookingRepository : BaseRepository, IBookingRepository
    {
        public BookingRepository(IConfiguration configuration) : base(configuration) { }

        // Get all booking records
        public async Task<List<Booking>> GetAllAsync()
        {
            using var connection = GetConnection();
            var result = await connection.QueryAsync<Booking>("sp_GetAllBooking", commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        // Get booking record by id
        public async Task<Booking?> GetByIdAsync(int id)
        {
            using var connection = GetConnection();
            var parameter = new DynamicParameters();
            parameter.Add("@BookingId", id);
            return await connection.QueryFirstOrDefaultAsync<Booking>("sp_GetBookingById", parameter, commandType: CommandType.StoredProcedure);
        }

        // Get by user id
        public async Task<List<Booking>> GetByUserIdAsync(int userId)
        {
            using var connection = GetConnection();
            var parameter = new DynamicParameters();
            parameter.Add("@UserId", userId);
            var result = await connection.QueryAsync<Booking>("sp_GetBookingsByUserId", parameter, commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        // Create booking record
        public async Task<int> CreateAsync(Booking model)
        {
            using var connection = GetConnection();

            var parameter = new DynamicParameters();

            parameter.Add("@UserId",model.UserId,DbType.Int32);

            parameter.Add("@RoomId",model.RoomId,DbType.Int32);

            parameter.Add("@FullName",model.FullName,DbType.String);

            parameter.Add("@Email",model.Email, DbType.String);

            parameter.Add("@Phone",model.Phone,DbType.String);

            parameter.Add("@CheckInDate",model.CheckInDate.Date,DbType.Date);

            parameter.Add("@CheckOutDate",model.CheckOutDate.Date,DbType.Date);

            parameter.Add("@Guests", model.Guests,DbType.Int32);

            parameter.Add("@SpecialRequest",model.SpecialRequest,DbType.String);

            parameter.Add("@TotalPrice",model.TotalPrice,DbType.Decimal,precision: 15,scale: 2);

            var bookingId = await connection.QuerySingleAsync<decimal>("sp_CreateBooking",parameter,commandType: CommandType.StoredProcedure);

            return Convert.ToInt32(bookingId);
        }

        // Update status
        public async Task<int> UpdateStatusAsync(int id, string status)
        {
            using var connection = GetConnection();
            var parameter = new DynamicParameters();
            parameter.Add("@BookingId", id);
            parameter.Add("@Status", status);
            return await connection.QuerySingleAsync<int>("sp_UpdateBookingStatus", parameter, commandType: CommandType.StoredProcedure);
        }

        // Delete booking record
        public async Task<int> DeleteAsync(int id)
        {
            using var connection = GetConnection();
            var parameter = new DynamicParameters();
            parameter.Add("@BookingId", id);
            return await connection.QuerySingleAsync<int>("sp_DeleteBooking", parameter, commandType: CommandType.StoredProcedure);
        }

        // Get conflict count
        public async Task<int> GetConflictCountAsync(int roomId, DateTime checkIn, DateTime checkOut, int? excludeBookingId = null)
        {
            using var connection = GetConnection();
            var parameter = new DynamicParameters();
            parameter.Add("@RoomId", roomId);
            parameter.Add("@CheckInDate", checkIn.Date);
            parameter.Add("@CheckOutDate", checkOut.Date);
            parameter.Add("@ExcludeBookingId", excludeBookingId);
            return await connection.QueryFirstOrDefaultAsync<int>("sp_CheckRoomAvailability", parameter, commandType: CommandType.StoredProcedure);
        }

        // Get rooms by category
        public async Task<List<Room>> GetRoomsByCategoryAsync(int roomCategoryId, int? excludeRoomId = null)
        {
            using var connection = GetConnection();
            var parameter = new DynamicParameters();
            parameter.Add("@RoomCategoryId", roomCategoryId);
            parameter.Add("@ExcludeRoomId", excludeRoomId);
            var result = await connection.QueryAsync<Room>("sp_GetRoomsByCategory", parameter, commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        // Propose room change
        public async Task<int> ProposeRoomChangeAsync(int bookingId, int proposedRoomId, string? note)
        {
            using var connection = GetConnection();
            var parameter = new DynamicParameters();
            parameter.Add("@BookingId", bookingId);
            parameter.Add("@ProposedRoomId", proposedRoomId);
            parameter.Add("@Note", note);
            return await connection.ExecuteScalarAsync<int>("sp_ProposeRoomChange", parameter, commandType: CommandType.StoredProcedure);
        }

        // Respond room change
        public async Task<int> RespondRoomChangeAsync(int bookingId, bool accept)
        {
            using var connection = GetConnection();
            var parameter = new DynamicParameters();
            parameter.Add("@BookingId", bookingId);
            parameter.Add("@Accept", accept);
            return await connection.ExecuteScalarAsync<int>("sp_RespondRoomChange", parameter, commandType: CommandType.StoredProcedure);
        }

        // Update email status
        public async Task<int> UpdateEmailStatusAsync(int bookingId, string emailStatus, string emailType)
        {
            using var connection = GetConnection();
            var parameter = new DynamicParameters();
            parameter.Add("@BookingId", bookingId);
            parameter.Add("@EmailStatus", emailStatus);
            parameter.Add("@EmailType", emailType);
            return await connection.ExecuteScalarAsync<int>("sp_UpdateBookingEmailStatus", parameter, commandType: CommandType.StoredProcedure);
        }

        // Cancel with refund
        public async Task<int> CancelWithRefundAsync(int bookingId, int refundPercentage, decimal refundAmount, string cancelledBy)
        {
            using var connection = GetConnection();
            var parameter = new DynamicParameters();
            parameter.Add("@BookingId", bookingId);
            parameter.Add("@RefundPercentage", refundPercentage);
            parameter.Add("@RefundAmount", refundAmount);
            parameter.Add("@CancelledBy", cancelledBy);
            return await connection.QueryFirstOrDefaultAsync<int>("sp_CancelBookingWithRefund", parameter, commandType: CommandType.StoredProcedure);
        }
    }
}
