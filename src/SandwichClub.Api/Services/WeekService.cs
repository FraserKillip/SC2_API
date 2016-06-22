using SandwichClub.Api.Repositories;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Services
{
    public class WeekService : BaseService<int, Week, IWeekRepository>, IWeekService
    {
        public WeekService(IWeekRepository weekRepository) : base(weekRepository)
        {
        }
    }
}
