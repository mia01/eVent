using Dapper;
using eventapp.Config;
using eventapp.Models;
using System.Collections.Generic;

namespace eventapp.Repositories
{
    public class TaskRepository: BaseRepository<Task>
    {
        public TaskRepository(Database databaseConfig) : base(databaseConfig)
        {
            Table = "Tasks";
            PrimaryKey = "Id";
        }

        public List<Task> GetByUserId(string userId)
        {
            using (var dbConnection = Connection)
            {
                string sQuery = $"SELECT * FROM {Table} WHERE CreatedBy = @UserId OR AssignedTo = @UserId";
                dbConnection.Open();
                return dbConnection.Query<Task>(sQuery, new { UserId = userId }).AsList();
            }
        }
    }
}
