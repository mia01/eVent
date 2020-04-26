using Dapper;
using eventapp.Domain.Config;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace eventapp.Domain.Repositories
{
    public class BaseRepository<T>
    {
        protected string Table;
        protected string PrimaryKey;
        
        private readonly Database _databaseConfig;
        private readonly string _connectionString;

        public BaseRepository(Database databaseConfig)
        {
            _databaseConfig = databaseConfig;
            _connectionString = GetConnectionString();
        }

        private string GetConnectionString()
        {
            return $@"server={_databaseConfig.Server};port={_databaseConfig.Port};database={_databaseConfig.Name};uid={_databaseConfig.User};pwd={_databaseConfig.Password};";
        }

        protected MySqlConnection Connection
        {
            get
            {
                return new MySqlConnection(_connectionString);
            }
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return await dbConnection.QueryAsync<T>($"SELECT * FROM {Table}");
            }
        }

        public async Task<T> GetById(long id)
        {
            using (var dbConnection = Connection)
            {
                string sQuery = $"SELECT * FROM {Table} WHERE {PrimaryKey} = @Id";
                dbConnection.Open();
                var result = await dbConnection.QueryAsync<T>(sQuery, new { Id = id });
                return result.FirstOrDefault();
            }
        }

        public async Task<int> Delete(long id)
        {
            using (MySqlConnection dbConnection = Connection)
            {
                string sQuery = $"DELETE FROM {Table} WHERE {PrimaryKey} = @Id";
                dbConnection.Open();
                return await dbConnection.ExecuteAsync(sQuery, new { Id = id });
            }
        }
        public async Task<long> Add(T item)
        {
            var type = item.GetType();
            var properties = type.GetProperties();

            var columnNames = new ArrayList();
            foreach (var property in properties)
            {
                if (property.Name != PrimaryKey)
                {
                    columnNames.Add(property.Name);
                }
            }

            var names = string.Join(", ", columnNames.ToArray());
            var values = string.Join(", @", columnNames.ToArray());

            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = $"INSERT INTO {Table} ({names}) VALUES (@{values}); SELECT LAST_INSERT_ID();";
                dbConnection.Open();
                return await dbConnection.ExecuteScalarAsync<long>(sQuery, item);
            }
        }

        public async Task<int> Update(T item)
        {
            var type = item.GetType();
            var properties = type.GetProperties();

            string updateString = "";
            foreach (var property in properties)
            {
                if (property.Name != PrimaryKey)
                {
                    updateString += property.Name + " = @" + property.Name;

                    if (property != properties.Last())
                    {
                        updateString += ",";
                    }
                }
            }
           
            var id = type.GetProperty(PrimaryKey).GetValue(item, null);
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = $"UPDATE {Table} SET {updateString} WHERE {PrimaryKey} = {id}";
                dbConnection.Open();
                return await dbConnection.ExecuteAsync(sQuery, item);
            }
        }
    }
}
