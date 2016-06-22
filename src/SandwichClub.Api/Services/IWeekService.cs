using System;
using System.Threading.Tasks;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Services
{
    public interface IWeekService : IBaseService<int, Week>
    {
        Task<Week> GetCurrentWeekAsync();

        int GetWeekId(DateTime date);
    }
}
