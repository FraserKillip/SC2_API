using System.Threading.Tasks;
using SandwichClub.Api.Dto;
using SandwichClub.Api.Repositories.Models;
using SandwichClub.Api.Services;

namespace SandwichClub.Api.Controllers.Mapper
{
    public class WeekMapper : BaseMapper<Week, WeekDto>
    {
        private readonly IWeekUserLinkService _weekUserLinkService;
        private readonly IMapper<WeekUserLink, WeekUserLinkDto> _weekUserLinkMapper;

        public WeekMapper(IWeekUserLinkService weekUserLinkService, IMapper<WeekUserLink, WeekUserLinkDto> weekUserLinkMapper)
        {
            _weekUserLinkService = weekUserLinkService;
            _weekUserLinkMapper = weekUserLinkMapper;
        }

        public override Task<Week> ToModelAsync(WeekDto week)
        {
            return Task.FromResult(new Week
            {
                WeekId = week.Id,
                ShopperUserId = week.Shopper,
                Cost = week.Cost
            });
        }

        public override async Task<WeekDto> ToDtoAsync(Week week)
        {
            var links = await _weekUserLinkService.GetByWeekIdAsync(week.WeekId);
            var linkDtos = await _weekUserLinkMapper.ToDtoAsync(links);

            return new WeekDto
            {
                Id = week.WeekId,
                Shopper = week.ShopperUserId,
                Cost = week.Cost,

                Links = linkDtos,
            };
        }
    }
}
