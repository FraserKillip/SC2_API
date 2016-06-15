using System.Collections.Generic;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Repositories
{
    public interface IWeekUserLinkRepository : IBaseRepository<int, WeekUserLink>
    {
        IEnumerable<WeekUserLink> GetByWeekId(int weekId);

        WeekUserLink GetByUserAndWeek(int userId, int weekId);
    }
}
