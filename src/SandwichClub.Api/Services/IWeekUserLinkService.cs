using System.Collections.Generic;
using System.Threading.Tasks;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Services
{
    public interface IWeekUserLinkService : IBaseService<WeekUserLinkId, WeekUserLink>
    {
        Task<IEnumerable<WeekUserLink>> GetByWeekIdAsync(int weekId);
        Task<IEnumerable<WeekUserLink>> GetByUserIdAsync(int userId, bool unpaidOnly = false);
        Task<int> CountForWeekAsync(int weekId);
        Task<decimal> GetSumPaidForUserAsync(int userId);
    }
}
