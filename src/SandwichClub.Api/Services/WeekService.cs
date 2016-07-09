using System;
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

        public Task<Week> GetCurrentWeekAsync()
        {
            return GetByIdAsync(GetWeekId(DateTime.Now));
        }

        public int GetWeekId(DateTime date)
        {
            // Get the date
            date = date.Date;
            // Change to the start of the week
            date = date.DayOfWeek != DayOfWeek.Sunday ? date.AddDays(DayOfWeek.Monday - date.DayOfWeek) : date.AddDays(-6);

            // Subtract Monday 5th of January 1970
            var timespan = date.Subtract(new DateTime(1970, 1, 7));
            return 1 + (int) timespan.TotalDays / 7;
        }
    }
}
