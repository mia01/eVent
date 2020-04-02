using eventapp.Config;
using eventapp.Models;

namespace eventapp.Repositories
{
    public class PriorityRepository: BaseRepository<Priority>
    {
        public PriorityRepository(Database databaseConfig) : base(databaseConfig)
        {
            Table = "Priority";
            PrimaryKey = "Id";
        }
    }
}
