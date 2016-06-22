using System.Threading.Tasks;
using SandwichClub.Api.Repositories;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Services
{
    public class WeekService : BaseService<int, Week, IWeekRepository>, IWeekService
    {
        public WeekService(IWeekRepository weekRepository) : base(weekRepository)
        {
        }

        public override async Task<Week> GetByIdAsync(int weekId)
        {
            var week = await Repository.GetByIdAsync(weekId);

            return week ?? new Week {WeekId = weekId};
        }

        public override Task<Week> InsertAsync(Week week)
        {
            return InsertOrUpdateAsync(week);
        }

        public override Task UpdateAsync(Week week)
        {
            return InsertOrUpdateAsync(week);
        }

        public async Task<Week> InsertOrUpdateAsync(Week week)
        {
            var existingWeek = await Repository.GetByIdAsync(week.WeekId);

            var exists = existingWeek != null;
            var delete = week.Cost <= 0.0 && week.ShopperUserId == null;

            if (exists && delete)
                await Repository.DeleteAsync(existingWeek);
            else if (exists)
                await Repository.UpdateAsync(week);
            else if (!delete)
                week = await Repository.InsertAsync(week);

            return week;
        }
    }
}
