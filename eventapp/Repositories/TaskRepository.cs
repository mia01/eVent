using eventapp.Config;
using eventapp.Models;

namespace eventapp.Repositories
{
    public class TaskRepository: BaseRepository<Task>
    {
        public TaskRepository(Database databaseConfig) : base(databaseConfig)
        {
            Table = "Tasks";
            PrimaryKey = "Id";
        }
    }
}
