using SandwichClub.Api.DTO;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Services.Mapper
{
    public class WeekUserLinkMapper : BaseMapper<WeekUserLink, WeekUserLinkDto>
    {
        public override WeekUserLink ToModel(WeekUserLinkDto link)
        {
            return new WeekUserLink
            {
                UserId = link.UserId,
                WeekId = link.WeekId,

                Paid = link.Paid,
                Slices = link.Slices,
            };
        }

        public override WeekUserLinkDto ToDto(WeekUserLink link)
        {
            return new WeekUserLinkDto
            {
                WeekId = link.WeekId,
                UserId = link.UserId,

                Paid = link.Paid,
                Slices = link.Slices,
            };
        }
    }
}
