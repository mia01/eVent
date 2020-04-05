using System;
using System.Collections;
using System.Collections.Generic;
using eventapp.Config;
using System.Data;
using System.Linq;
using Dapper;
using MySql.Data.MySqlClient;

namespace eventapp.Repositories
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

        protected MySqlConnection Connection => new MySqlConnection(_connectionString);

        public IEnumerable<T> GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<T>($"SELECT * FROM {Table}");
            }
        }

        public T GetById(long id)
        {
            using (MySqlConnection dbConnection = Connection)
            {
                string sQuery = $"SELECT * FROM {Table} WHERE {PrimaryKey} = @Id";
                dbConnection.Open();
                return dbConnection.Query<T>(sQuery, new { Id = id }).FirstOrDefault();
            }
        }

        public void Delete(long id)
        {
            using (MySqlConnection dbConnection = Connection)
            {
                string sQuery = $"DELETE FROM {Table} WHERE {PrimaryKey} = @Id";
                dbConnection.Open();
                dbConnection.Execute(sQuery, new { Id = id });
            }
        }
        public long Add(T item)
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
                return dbConnection.ExecuteScalar<long>(sQuery, item);
            }
        }

        public int Update(T item)
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
                return dbConnection.Execute(sQuery, item);
            }
        }
    }
}
