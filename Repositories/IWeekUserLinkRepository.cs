using System.Collections.Generic;
using SC2_API.Repositories.Models;

namespace SC2_API.Repositories
{
    public interface IWeekUserLinkRepository : IBaseRepository<int, WeekUserLink>
    {
        IEnumerable<WeekUserLink> GetByWeekId(int weekId);

        WeekUserLink GetByUserAndWeek(int userId, int weekId);
    }
}
