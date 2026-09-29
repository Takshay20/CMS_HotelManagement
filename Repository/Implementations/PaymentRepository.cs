using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using Dapper;
using System.Data;

namespace CMS_HotelBooking.Repository.Implementations
{
    public class PaymentRepository : BaseRepository, IPaymentRepository
    {
        public PaymentRepository(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<Payment?> GetByBookingIdAsync(int bookingId)
        {
            using var connection = GetConnection();

            var parameters = new DynamicParameters();

            parameters.Add(
                "@BookingId",
                bookingId,
                DbType.Int32
            );

            return await connection.QueryFirstOrDefaultAsync<Payment>(
                "sp_GetPaymentByBookingId",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<Payment?> GetByOrderIdAsync(string orderId)
        {
            using var connection = GetConnection();

            var parameters = new DynamicParameters();

            parameters.Add(
                "@OrderId",
                orderId,
                DbType.String
            );

            return await connection.QueryFirstOrDefaultAsync<Payment>(
                "sp_GetPaymentByOrderId",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> CreateAsync(Payment payment)
        {
            using var connection = GetConnection();

            var parameters = new DynamicParameters();

            parameters.Add(
                "@BookingId",
                payment.BookingId,
                DbType.Int32
            );

            parameters.Add(
                "@UserId",
                payment.UserId,
                DbType.Int32
            );

            parameters.Add(
                "@Amount",
                payment.Amount,
                DbType.Decimal,
                precision: 15,
                scale: 2
            );

            parameters.Add(
                "@OrderId",
                payment.OrderId,
                DbType.String
            );

            parameters.Add(
                "@GatewayPaymentId",
                payment.GatewayPaymentId,
                DbType.String
            );

            parameters.Add(
                "@PaymentStatus",
                payment.PaymentStatus,
                DbType.String
            );

            parameters.Add(
                "@PaymentDate",
                payment.PaymentDate,
                DbType.DateTime
            );

            parameters.Add(
                "@FailureReason",
                payment.FailureReason,
                DbType.String
            );

            return await connection.QuerySingleAsync<int>(
                "sp_CreatePayment",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> UpdatePaymentOrderAsync(
            int paymentId,
            decimal amount,
            string orderId,
            string paymentStatus)
        {
            using var connection = GetConnection();

            var parameters = new DynamicParameters();

            parameters.Add(
                "@PaymentId",
                paymentId,
                DbType.Int32
            );

            parameters.Add(
                "@Amount",
                amount,
                DbType.Decimal,
                precision: 15,
                scale: 2
            );

            parameters.Add(
                "@OrderId",
                orderId,
                DbType.String
            );

            parameters.Add(
                "@PaymentStatus",
                paymentStatus,
                DbType.String
            );

            return await connection.QuerySingleAsync<int>(
                "sp_UpdatePaymentOrder",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> UpdateStatusAsync(
            int paymentId,
            string paymentStatus,
            string? gatewayPaymentId,
            DateTime? paymentDate,
            string? failureReason)
        {
            using var connection = GetConnection();

            var parameters = new DynamicParameters();

            parameters.Add(
                "@PaymentId",
                paymentId,
                DbType.Int32
            );

            parameters.Add(
                "@PaymentStatus",
                paymentStatus,
                DbType.String
            );

            parameters.Add(
                "@GatewayPaymentId",
                gatewayPaymentId,
                DbType.String
            );

            parameters.Add(
                "@PaymentDate",
                paymentDate,
                DbType.DateTime
            );

            parameters.Add(
                "@FailureReason",
                failureReason,
                DbType.String
            );

            return await connection.ExecuteScalarAsync<int>(
                "sp_UpdatePaymentStatus",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<Payment?> GetByPaymentIdAsync(int paymentId)
        {
            using var connection = GetConnection();

            var parameters = new DynamicParameters();

            parameters.Add(
                "@PaymentId",
                paymentId,
                DbType.Int32
            );

            return await connection.QueryFirstOrDefaultAsync<Payment>(
                "sp_GetPaymentByPaymentId",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> UpdatePaymentAmountAsync(
    int paymentId,
    decimal amount)
        {
            using var connection = GetConnection();

            var parameters = new DynamicParameters();

            parameters.Add(
                "@PaymentId",
                paymentId,
                DbType.Int32
            );

            parameters.Add(
                "@Amount",
                amount,
                DbType.Decimal,
                precision: 15,
                scale: 2
            );

            return await connection.ExecuteScalarAsync<int>(
                "sp_UpdatePaymentAmount",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}