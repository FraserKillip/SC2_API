using System.Collections.Generic;
using System.Threading.Tasks;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Repositories
{
    public interface IWeekUserLinkRepository : IBaseRepository<WeekUserLinkId, WeekUserLink>
    {
        Task<IList<WeekUserLink>> GetByWeekIdAsync(int weekId);
    }
}
