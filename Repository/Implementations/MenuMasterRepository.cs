using CMS_HotelBooking.Models;
using CMS_HotelBooking.Repository.Interfaces;
using Dapper;
using System.Data;

namespace CMS_HotelBooking.Repository.Implementations
{
    public class MenuMasterRepository : BaseRepository, IMenuMasterRepository
    {
        public MenuMasterRepository(IConfiguration configuration) : base(configuration) { }

        // Get all menu master records
        public async Task<List<MenuMaster>> GetAllAsync()
        {
            using var connection = GetConnection();
            var result = await connection.QueryAsync<MenuMaster>("sp_GetAllMenuMaster", commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        // Get active
        public async Task<List<MenuMaster>> GetActiveAsync()
        {
            using var connection = GetConnection();
            var result = await connection.QueryAsync<MenuMaster>("sp_GetActiveMenuMaster", commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        // Get menu master record by id
        public async Task<MenuMaster?> GetByIdAsync(int id)
        {
            using var connection = GetConnection();
            var parameter = new DynamicParameters();
            parameter.Add("@MenuMasterId", id);
            return await connection.QueryFirstOrDefaultAsync<MenuMaster>("sp_GetMenuMasterById", parameter, commandType: CommandType.StoredProcedure);
        }

        // Save menu master record
        public async Task<int> SaveAsync(MenuMaster model)
        {
            using var connection = GetConnection();
            var parameter = new DynamicParameters();
            parameter.Add("@MenuMasterId", model.MenuMasterId);
            parameter.Add("@Title", model.Title);
            parameter.Add("@Url", model.Url);
            parameter.Add("@DisplayOrder", model.DisplayOrder);
            parameter.Add("@IsActive", model.IsActive);
            return await connection.QueryFirstOrDefaultAsync<int>("sp_SaveMenuMaster", parameter, commandType: CommandType.StoredProcedure);
        }

        // Delete menu master record
        public async Task<int> DeleteAsync(int id)
        {
            using var connection = GetConnection();
            var parameter = new DynamicParameters();
            parameter.Add("@MenuMasterId", id);
            return await connection.QueryFirstOrDefaultAsync<int>("sp_DeleteMenuMaster", parameter, commandType: CommandType.StoredProcedure);
        }
    }
}
