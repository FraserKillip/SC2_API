using System;
using System.Threading.Tasks;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Services
{
    public interface IWeekService : IBaseService<int, Week>
    {
        /// <summary>
        /// Gets the week for the current date
        /// </summary>
        Task<Week> GetCurrentWeekAsync();

        /// <summary>
        /// Gets the id of a week for the given date
        /// </summary>
        int GetWeekId(DateTime date);

        Task<Week> SubscibeToWeek(int weekId, int userId, int slices);
    }
}
