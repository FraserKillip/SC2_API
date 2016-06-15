using System.Collections.Generic;
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
            return await _dbSet.FirstOrDefaultAsync(wul => wul.UserId == id.UserId && wul.WeekId == id.UserId);
        }

        public async Task<IList<WeekUserLink>> GetByWeekIdAsync(int weekId)
        {
            return await _dbSet.Include(wul => wul.WeekId == weekId).ToListAsync();
        }
    }
}
