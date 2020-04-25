using Dapper;
using eventapp.Config;
using eventapp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eventapp.Repositories
{
    public class UserFriendRepository: BaseRepository<UserFriend>
    {
        public UserFriendRepository(Database databaseConfig) : base(databaseConfig)
        {
            Table = "UserFriend";
            PrimaryKey = "Id";
        }

        public async Task<List<UserFriend>> GetUserFriendRequests(string userId)
        {
            using (var dbConnection = Connection)
            {
                string query = $"SELECT * FROM {Table} WHERE (UserId = @UserId AND Accepted = 0)";
                dbConnection.Open();
                var result = await dbConnection.QueryAsync<UserFriend>(query, new { UserId = userId });
                return result.AsList();
            }
        }

        public async Task<List<UserFriend>> GetUserFriendInvites(string userId)
        {
            using (var dbConnection = Connection)
            {
                string query = $"SELECT * FROM {Table} WHERE (UserFriendId = @UserId AND Accepted = 0)";
                dbConnection.Open();
                var result = await dbConnection.QueryAsync<UserFriend>(query, new { UserId = userId });
                return result.AsList();
            }
        }

        public async Task<List<UserFriend>> GetUserFriends(string userId)
        {
            using (var dbConnection = Connection)
            {
                string query = $"SELECT * FROM {Table} WHERE (UserId = @UserId AND Accepted = 1)" +
                    $" OR (UserFriendId = @UserId AND Accepted = 1)";
                dbConnection.Open();
                var result = await dbConnection.QueryAsync<UserFriend>(query, new { UserId = userId });
                return result.AsList();
            }
        }
    }
}
