using System.Collections.Generic;
using System.Threading.Tasks;
using SandwichClub.Api.DTO;
using SandwichClub.Api.Services.Mapper;
using SandwichClub.Api.Repositories;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Services
{
    public class WeekUserLinkService : BaseService<WeekUserLinkId, WeekUserLink, WeekUserLinkDto, IWeekUserLinkRepository>, IWeekUserLinkService
    {
        private readonly IUserService _userService;

        public WeekUserLinkService(IWeekUserLinkRepository weekUserLinkRepository, IMapper<WeekUserLink, WeekUserLinkDto> mapper, IUserService userService) : base(weekUserLinkRepository, mapper)
        {
            _userService = userService;
        }

        public async Task<IEnumerable<WeekUserLinkDto>> GetByWeekIdAsync(int weekId)
        {
            var items = await Repository.GetByWeekIdAsync(weekId);
            return await ToDtosAsync(items);
        }

        public override Task<WeekUserLinkDto> InsertAsync(WeekUserLinkDto linkDto)
        {
            return InsertOrUpdateAsync(linkDto);
        }

        public override Task UpdateAsync(WeekUserLinkDto linkDto)
        {
            return InsertOrUpdateAsync(linkDto);
        }

        public async Task<WeekUserLinkDto> InsertOrUpdateAsync(WeekUserLinkDto linkDto)
        {
            var link = ToModel(linkDto);
            var id = new WeekUserLinkId {UserId = linkDto.UserId, WeekId = linkDto.WeekId};
            var existingLink = await Repository.GetByIdAsync(id);
            var exists = existingLink != null;

            // Check if we should delete this link
            var delete = link.Paid <= 0 && link.Slices <= 0;

            if (exists && delete)
                await Repository.DeleteAsync(id);
            else if (exists)
                await Repository.UpdateAsync(link);
            else if (!delete)
                link = await Repository.InsertAsync(link);

            return ToDto(link);
        }

        protected override async Task HydrateDtoAsync(WeekUserLink item, WeekUserLinkDto dto)
        {
            dto.User = await _userService.GetByIdAsync(dto.UserId);
        }
    }
}