using AutoMapper;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Repositories
{
    public class WeekRepository : BaseRepository<int, Week>, IWeekRepository
    {
        public WeekRepository(ScContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
