using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SandwichClub.Api.Repositories;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Services
{
    public class WeekService : SaveOnlyBaseService<int, Week, IWeekRepository>, IWeekService
    {
        private readonly IWeekUserLinkService _weekUserLinkService;
        private readonly IUserService _userService;
        private readonly ITelemetryService _telemetryService;

        public WeekService(IWeekRepository weekRepository, ILogger<WeekService> logger, IWeekUserLinkService weekUserLinkService, ITelemetryService telemetryService, IUserService userService) : base(weekRepository, logger)
        {
            _weekUserLinkService = weekUserLinkService;
            _telemetryService = telemetryService;
            _userService = userService;
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

        public async Task<decimal> GetAmountToPayPerPersonAsync(int weekId)
        {

            var week = await GetByIdAsync(weekId);
            var linkCount = await _weekUserLinkService.CountForWeekAsync(weekId);

            if (linkCount == 0)
                return 0m;

            return (decimal) week.Cost/linkCount;
        }

        public async Task<IEnumerable<WeekUserLink>> MarkAllLinksAsPaidForUserAsync(int userId)
        {
            // Get them weeks
            var totalAmountToBePaid = await GetTotalCostsForUserAsync(userId);
            var totalAmountPaid = await _weekUserLinkService.GetSumPaidForUserAsync(userId);

            var amountOwed = totalAmountToBePaid - totalAmountPaid;

            var unpaidWeeks = await _weekUserLinkService.GetByUserIdAsync(userId, unpaidOnly:true);

            var currentWeekId = GetWeekId(DateTime.Now);

            var remainingOwed = amountOwed;
            foreach (var week in unpaidWeeks)
            {
                if (week.WeekId >= currentWeekId)
                    continue;

                // Get the $$$
                var amountToPay = await GetAmountToPayPerPersonAsync(week.WeekId);
                // Make sure we don't overpay
                if (remainingOwed - amountToPay <= 0)
                {
                    amountToPay = remainingOwed;
                    remainingOwed = 0;
                }
                else
                {
                    remainingOwed -= amountToPay;
                }
                week.Paid = (double) amountToPay;
                // Do it
                await _weekUserLinkService.SaveAsync(week);

                if (remainingOwed <= 0)
                    break;
            }

            var user = await _userService.GetByIdAsync(userId);
            _telemetryService.TrackEvent("Payment",
                new
                {
                    username = $"{user.FirstName} {user.LastName}",
                    totalAmountToBePaid,
                    totalAmountPaid,
                    amountOwed,
                    remainingOwed
                });

            return unpaidWeeks;
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

        public async Task<Week> SubscibeToWeek(int weekId, int userId, int slices)
        {
            var link = await _weekUserLinkService.GetByIdAsync(new WeekUserLinkId { WeekId = weekId, UserId = userId });
            if (link == null)
                link = new WeekUserLink { WeekId = weekId, UserId = userId };

            link.Slices = slices;

            await _weekUserLinkService.SaveAsync(link);

            return await GetByIdAsync(weekId);
        }

        public async Task<decimal> GetTotalCostsForUserAsync(int userId)
        {
            var weekLinks = await _weekUserLinkService.GetByUserIdAsync(userId);

            var totalCost = 0m;

            var currentWeekId = GetWeekId(DateTime.Now);

            foreach (var link in weekLinks)
            {
                if (link.WeekId >= currentWeekId)
                    continue;
                totalCost += await GetAmountToPayPerPersonAsync(link.WeekId);
            }

            return totalCost;
        }
    }
}
