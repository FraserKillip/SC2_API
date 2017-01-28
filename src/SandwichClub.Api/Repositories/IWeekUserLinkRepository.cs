using System.Collections.Generic;
using System.Threading.Tasks;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Repositories
{
    public interface IWeekUserLinkRepository : IBaseRepository<WeekUserLinkId, WeekUserLink>
    {
        Task<IEnumerable<WeekUserLink>> GetByWeekIdAsync(int weekId);
        Task<IEnumerable<WeekUserLink>> GetByUserIdAsync(int userId);
        Task<int> CountForWeekAsync(int weekId);
        Task<decimal> GetSumPaidForUserAsync(int userId);
    }
}
