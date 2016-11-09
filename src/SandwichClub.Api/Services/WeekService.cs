using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SandwichClub.Api.Repositories;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Services
{
    public class WeekService : SaveOnlyBaseService<int, Week, IWeekRepository>, IWeekService
    {
        public WeekService(IWeekRepository weekRepository, ILogger<WeekService> logger) : base(weekRepository, logger)
        {
        }

        /// <summary>
        /// Provides a persisted week or the default value
        /// </summary>
        public override async Task<Week> GetByIdAsync(int weekId)
        {
            var week = await Repository.GetByIdAsync(weekId);

            return week ?? new Week {WeekId = weekId};
        }

        /// <summary>
        /// Checks if the cost of the week is the default value and that there is no shopper
        /// </summary>
        protected override bool SaveShouldDelete(Week week)
        {
            return week.Cost <= 0.0 && week.ShopperUserId == null;
        }

                public override int GetId(Week t)
        {
            return t.WeekId;
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

        public async Task<Week> UpdateWeek(int weekId, int? shopperId, float? cost) {
            var week = await GetByIdAsync(weekId);

            if (shopperId.HasValue)
            {
                week.ShopperUserId = shopperId.Value;
            }

            if (cost.HasValue)
            {
                week.Cost = cost.Value;
            }

            return await SaveAsync(week);
        }
    }
}
