using eventapp.Domain.Config;
using eventapp.Domain.Models;

namespace eventapp.Domain.Repositories
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
