using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using Dapper;
using System.Data;

namespace CMS_HotelBooking.Repository.Implementations
{
    public class ContactMessageRepository : BaseRepository, IContactMessageRepository
    {
        public ContactMessageRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<List<ContactMessage>> GetAllAsync()
        {
            using var connection = GetConnection();
            var result = await connection.QueryAsync<ContactMessage>("sp_GetAllContactMessage", commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        public async Task<ContactMessage?> GetByIdAsync(int id)
        {
            using var connection = GetConnection();
            var parameter = new DynamicParameters();
            parameter.Add("@ContactMessageId", id);
            return await connection.QueryFirstOrDefaultAsync<ContactMessage>("sp_GetContactMessageById", parameter, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> SubmitAsync(ContactMessage model)
        {
            using var connection = GetConnection();

            var parameter = new DynamicParameters();

            parameter.Add("@Name", model.Name);
            parameter.Add("@Email", model.Email);
            parameter.Add("@Phone", model.Phone);
            parameter.Add("@Subject", model.Subject);
            parameter.Add("@Message", model.Message);

            var result = await connection.QueryFirstOrDefaultAsync<decimal>(
                "sp_SubmitContactMessage",
                parameter,
                commandType: CommandType.StoredProcedure
            );

            return Convert.ToInt32(result);
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var connection = GetConnection();
            var parameter = new DynamicParameters();
            parameter.Add("@ContactMessageId", id);
            return await connection.ExecuteAsync("sp_DeleteContactMessage", parameter, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> ReplyAsync(int id, string adminReply)
        {
            using var connection = GetConnection();
            var parameter = new DynamicParameters();
            parameter.Add("@ContactMessageId", id);
            parameter.Add("@AdminReply", adminReply);
            return await connection.ExecuteAsync("sp_ReplyToContactMessage", parameter, commandType: CommandType.StoredProcedure);
        }

        public async Task<List<ContactMessage>> GetByEmailAsync(string email)
        {
            using var connection = GetConnection();
            var parameter = new DynamicParameters();
            parameter.Add("@Email", email);
            var result = await connection.QueryAsync<ContactMessage>("sp_GetContactMessagesByEmail", parameter, commandType: CommandType.StoredProcedure);
            return result.ToList();
        }
    }
}
