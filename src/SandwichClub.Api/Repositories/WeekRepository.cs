using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Repositories
{
    public class WeekRepository : BaseRepository<int, Week>, IWeekRepository
    {
        public WeekRepository(SC2Context context) : base(context)
        {
        }

        public override async Task<Week> GetByIdAsync(int id)
        {
            if (id == 0)
                return null;
            return await _dbSet.FirstOrDefaultAsync(w => w.WeekId == id);
        }
    }
}
