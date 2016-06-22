using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Repositories
{
    public class WeekUserLinkRepository : BaseRepository<WeekUserLinkId, WeekUserLink>, IWeekUserLinkRepository
    {
        public WeekUserLinkRepository(SC2Context context) : base(context)
        {
        }

        public override async Task<WeekUserLink> GetByIdAsync(WeekUserLinkId id)
        {
            if (id.UserId == 0 || id.WeekId == 0)
                return null;
            return await DbSet.FirstOrDefaultAsync(wul => wul.UserId == id.UserId && wul.WeekId == id.UserId);
        }

        public override async Task<IEnumerable<WeekUserLink>> GetByIdsAsync(IEnumerable<WeekUserLinkId> ids)
        {
            var linkTasks = ids.Select(GetByIdAsync);
            var links = await Task.WhenAll(linkTasks);

            return links.Where(link => link != null);
        }

        public async Task<IEnumerable<WeekUserLink>> GetByWeekIdAsync(int weekId)
        {
            return await DbSet.Where(wul => wul.WeekId == weekId).ToListAsync();
        }
    }
}
