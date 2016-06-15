using System.Collections.Generic;
using System.Threading.Tasks;
using SandwichClub.Api.DTO;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Services
{
    public interface IWeekUserLinkService : IBaseService<WeekUserLinkId, WeekUserLinkDto>
    {
        Task<IEnumerable<WeekUserLinkDto>> GetByWeekIdAsync(int weekId);
    }
}
