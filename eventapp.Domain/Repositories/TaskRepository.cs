using Dapper;
using eventapp.Domain.Config;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eventapp.Domain.Repositories
{
    public class TaskRepository: BaseRepository<Models.Task>
    {
        public TaskRepository(Database databaseConfig) : base(databaseConfig)
        {
            Table = "Tasks";
            PrimaryKey = "Id";
        }

        public async Task<List<Models.Task>> GetByUserId(string userId)
        {
            using (var dbConnection = Connection)
            {
                string sQuery = $"SELECT * FROM {Table} WHERE CreatedBy = @UserId OR AssignedTo = @UserId";
                dbConnection.Open();
                var result = await dbConnection.QueryAsync<Models.Task>(sQuery, new { UserId = userId });
                return result.AsList();
            }
        }

        public async Task<List<Models.Task>> GetTasksDueToday()
        {
            var today = DateTime.Today;
            var nextDay = DateTime.Today.AddDays(1);
            using (var dbConnection = Connection)
            {
                string sQuery = $"SELECT * FROM {Table} WHERE DueDate >= @Today AND DueDate <= @NextDay";
                dbConnection.Open();
                var result = await dbConnection.QueryAsync<Models.Task>(sQuery, new { Today = today, NextDay = nextDay });
                return result.AsList();
            }
        }
    }
}
