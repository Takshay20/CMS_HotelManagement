using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using Dapper;
using System.Data;

namespace CMS_HotelBooking.Repository.Implementations
{
    public class UsersRepository : BaseRepository, IUsersRepository
    {
        public UsersRepository(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<Users?> GetByEmailAsync(string email)
        {
            using var connection = GetConnection();

            var parameter = new DynamicParameters();

            parameter.Add("@Email", email);

            return await connection.QueryFirstOrDefaultAsync<Users>(
                "sp_GetUserByEmail",
                parameter,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<Users?> GetByIdAsync(int id)
        {
            using var connection = GetConnection();

            var parameter = new DynamicParameters();

            parameter.Add("@UserId", id);

            return await connection.QueryFirstOrDefaultAsync<Users>(
                "sp_GetUserById",
                parameter,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<List<Users>> GetAllAsync()
        {
            using var connection = GetConnection();

            var result = await connection.QueryAsync<Users>(
                "sp_GetAllUsers",
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }

        public async Task<int> RegisterAsync(
    Users model,
    string role)
        {
            using var connection = GetConnection();

            var parameter = new DynamicParameters();

            parameter.Add(
                "@FullName",
                model.FullName
            );

            parameter.Add(
                "@Email",
                model.Email
            );

            parameter.Add(
                "@CountryCode",
                model.CountryCode
            );

            parameter.Add(
                "@Phone",
                model.Phone
            );

            parameter.Add(
                "@PasswordHash",
                model.PasswordHash
            );

            parameter.Add(
                "@Role",
                role
            );

            return await connection.QueryFirstOrDefaultAsync<int>(
                "sp_RegisterUser",
                parameter,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> SaveResetCodeAsync(
    int userId,
    string resetCode,
    DateTime expiry)
        {
            using var connection = GetConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@UserId", userId);
            parameters.Add("@ResetCode", resetCode);
            parameters.Add("@ResetCodeExpiry", expiry);

            return await connection.QueryFirstOrDefaultAsync<int>(
                "sp_SaveResetCode",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<bool> VerifyResetCodeAsync(
            int userId,
            string resetCode)
        {
            using var connection = GetConnection();

            var parameter = new DynamicParameters();

            parameter.Add(
                "@UserId",
                userId
            );

            parameter.Add(
                "@ResetCode",
                resetCode
            );

            var result =
                await connection.QueryFirstOrDefaultAsync<int>(
                    "sp_VerifyPasswordResetCode",
                    parameter,
                    commandType: CommandType.StoredProcedure
                );

            return result == 1;
        }

        public async Task<int> ClearResetCodeAsync(
            int userId)
        {
            using var connection = GetConnection();

            var parameter = new DynamicParameters();

            parameter.Add(
                "@UserId",
                userId
            );

            return await connection.ExecuteAsync(
                "sp_ClearPasswordResetCode",
                parameter,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> UpdatePasswordAsync(
            int userId,
            string passwordHash)
        {
            using var connection = GetConnection();

            var parameter = new DynamicParameters();

            parameter.Add(
                "@UserId",
                userId
            );

            parameter.Add(
                "@PasswordHash",
                passwordHash
            );

            return await connection.QueryFirstOrDefaultAsync<int>(
                "sp_UpdateUserPassword",
                parameter,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> ToggleActiveAsync(int id)
        {
            using var connection = GetConnection();

            var parameter = new DynamicParameters();

            parameter.Add("@UserId",id,DbType.Int32);

            return await connection.QueryFirstOrDefaultAsync<int>(
                "sp_ToggleUserActive",
                parameter,
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task<int> DeleteAsync(int id)
        {
            using var connection = GetConnection();

            var parameter = new DynamicParameters();

            parameter.Add( "@UserId",id);

            return await connection.QueryFirstOrDefaultAsync<int>(
                "sp_DeleteUser",
                parameter,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}