using Dapper;
using eventapp.Domain.Config;
using eventapp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eventapp.Domain.Repositories
{
    public class EventRepository: BaseRepository<Event>
    {
        public EventRepository(Database databaseConfig) : base(databaseConfig)
        {
            Table = "Events";
            PrimaryKey = "Id";
        }

        public async Task<List<Event>> GetByUserId(string userId)
        {
            using (var dbConnection = Connection)
            {
                string sQuery = $"SELECT * FROM {Table} WHERE CreatedBy = @UserId";
                dbConnection.Open();
                var result = await dbConnection.QueryAsync<Event>(sQuery, new { UserId = userId });
                return result.AsList();
            }
        }

        public async Task<List<Event>> GetEventsStartingToday()
        {
            var today = DateTime.Today;
            var nextDay = DateTime.Today.AddDays(1);
            using (var dbConnection = Connection)
            {
                string sQuery = $"SELECT * FROM {Table} WHERE StartDate >= @Today AND StartDate <= @NextDay";
                dbConnection.Open();
                var result = await dbConnection.QueryAsync<Event>(sQuery, new { Today = today, NextDay = nextDay });
                return result.AsList();
            }
        }
    }
}
