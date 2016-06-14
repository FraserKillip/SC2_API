using System.Collections.Generic;
using SC2_API.DTO;

namespace SC2_API.Services
{
    public interface IWeekUserLinkService
    {
        WeekUserLinkDto updateLink(WeekUserLinkDto link, int weekId);
    }
}
