using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Repositories
{
    public class WeekUserLinkRepository : BaseRepository<WeekUserLinkId, WeekUserLink>, IWeekUserLinkRepository
    {
        public WeekUserLinkRepository(ScContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public override object[] GetKeys(WeekUserLinkId id)
            => new object[] { id.WeekId, id.UserId };

        public async Task<IEnumerable<WeekUserLink>> GetByWeekIdAsync(int weekId)
        {
            return await DbSet.Where(wul => wul.WeekId == weekId).ToListAsync();
        }

        public async Task<IEnumerable<WeekUserLink>> GetByUserIdAsync(int userId)
        {
            return await DbSet.Where(wul => wul.UserId == userId).ToListAsync();
        }

        public async Task<int> CountForWeekAsync(int weekId)
        {
            return await DbSet.Where(wul => wul.WeekId == weekId).CountAsync();
        }

        public async Task<decimal> GetSumPaidForUserAsync(int userId)
        {
            return (decimal) await DbSet.Where(wul => wul.UserId == userId).SumAsync(wul => wul.Paid);
        }
    }
}
