using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
