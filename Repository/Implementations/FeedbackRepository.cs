using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using Dapper;
using System.Data;

namespace CMS_HotelBooking.Repository.Implementations
{
    public class FeedbackRepository : BaseRepository, IFeedbackRepository
    {
        public FeedbackRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<List<Feedback>> GetAllAsync()
        {
            using var connection = GetConnection();
            var result = await connection.QueryAsync<Feedback>("sp_GetAllFeedback", commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        public async Task<List<Feedback>> GetApprovedAsync(int top = 12)
        {
            using var connection = GetConnection();
            var parameter = new DynamicParameters();
            parameter.Add("@Top", top);
            var result = await connection.QueryAsync<Feedback>("sp_GetApprovedFeedback", parameter, commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        public async Task<Feedback?> GetByIdAsync(int id)
        {
            using var connection = GetConnection();
            var parameter = new DynamicParameters();
            parameter.Add("@FeedbackId", id);
            return await connection.QueryFirstOrDefaultAsync<Feedback>("sp_GetFeedbackById", parameter, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> SubmitAsync(Feedback model)
        {
            using var connection = GetConnection();
            var parameter = new DynamicParameters();
            parameter.Add("@UserId", model.UserId);
            parameter.Add("@Name", model.Name);
            parameter.Add("@Designation", model.Designation);
            parameter.Add("@ImagePath", model.ImagePath);
            parameter.Add("@Message", model.Message);
            parameter.Add("@Rating", model.Rating);
            return await connection.QueryFirstOrDefaultAsync<int>("sp_SubmitFeedback", parameter, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> ToggleApprovalAsync(int id)
        {
            using var connection = GetConnection();
            var parameter = new DynamicParameters();
            parameter.Add("@FeedbackId", id);
            return await connection.QueryFirstOrDefaultAsync<int>("sp_ToggleFeedbackApproval", parameter, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var connection = GetConnection();
            var parameter = new DynamicParameters();
            parameter.Add("@FeedbackId", id);
            return await connection.QueryFirstOrDefaultAsync<int>("sp_DeleteFeedback", parameter, commandType: CommandType.StoredProcedure);
        }
    }
}
