using System.Collections.Generic;
using SandwichClub.Api.DTO;

namespace SandwichClub.Api.Services
{
    public interface IWeekUserLinkService
    {
        WeekUserLinkDto updateLink(WeekUserLinkDto link, int weekId);
    }
}
