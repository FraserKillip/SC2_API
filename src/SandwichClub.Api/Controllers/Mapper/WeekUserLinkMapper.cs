using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SandwichClub.Api.Dto;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Controllers.Mapper
{
    public class WeekUserLinkMapper : BaseMapper<WeekUserLink, WeekUserLinkDto>
    {
        public override Task<WeekUserLink> ToModelAsync(WeekUserLinkDto link)
        {
            return Task.FromResult(new WeekUserLink
            {
                UserId = link.UserId,
                WeekId = link.WeekId,

                Paid = link.Paid,
                Slices = link.Slices,
            });
        }

        public override Task<WeekUserLinkDto> ToDtoAsync(WeekUserLink link)
        {
            return Task.FromResult(new WeekUserLinkDto
            {
                WeekId = link.WeekId,
                UserId = link.UserId,

                Paid = link.Paid,
                Slices = link.Slices,
            });
        }
    }
}
