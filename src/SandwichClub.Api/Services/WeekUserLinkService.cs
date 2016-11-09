using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SandwichClub.Api.Repositories;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Services
{
    public class WeekUserLinkService : SaveOnlyBaseService<WeekUserLinkId, WeekUserLink, IWeekUserLinkRepository>, IWeekUserLinkService
    {
        public WeekUserLinkService(IWeekUserLinkRepository weekUserLinkRepository, ILogger<WeekUserLinkService> logger) : base(weekUserLinkRepository, logger)
        {
        }

        public Task<IEnumerable<WeekUserLink>> GetByWeekIdAsync(int weekId)
        {
            return Repository.GetByWeekIdAsync(weekId);
        }

        public async Task<IEnumerable<WeekUserLink>> GetByUserIdAsync(int userId, bool unpaidOnly = false)
        {
            var task = Repository.GetByUserIdAsync(userId);
            if (unpaidOnly) {
                var data = await task;
                return data.Where(w => w.Paid == 0);
            }
            return await task;
        }

        protected override bool SaveShouldDelete(WeekUserLink link)
        {
            return link.Paid <= 0 && link.Slices <= 0;
        }

        public override WeekUserLinkId GetId(WeekUserLink link)
        {
            return new WeekUserLinkId { WeekId = link.WeekId, UserId = link.UserId };
        }
    }
}
