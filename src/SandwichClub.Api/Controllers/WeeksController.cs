using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SandwichClub.Api.Controllers.Mapper;
using SandwichClub.Api.DTO;
using SandwichClub.Api.Repositories.Models;
using SandwichClub.Api.Services;

namespace SandwichClub.Api.Controllers
{
    [Route("api/[controller]")]
    public class WeeksController : ControllerBase<int, Week, WeekDto, IWeekService>
    {
        private readonly IWeekUserLinkService _weekUserLinkService;
        private readonly IMapper<WeekUserLink, WeekUserLinkDto> _weekUserLinkMapper;

        public WeeksController(IWeekService weekService, IWeekUserLinkService weekUserLinkService, IMapper<Week, WeekDto> weekMapper, IMapper<WeekUserLink, WeekUserLinkDto> weekUserLinkMapper, IMapper<WeekUserLink, WeekUserLinkDto> weekUserLinkMapper1) : base(weekService, weekMapper)
        {
            _weekUserLinkService = weekUserLinkService;
            _weekUserLinkMapper = weekUserLinkMapper1;
        }

        /// <summary>
        /// Don't allow deletions
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public override Task Delete(int id)
        {
            return Task.CompletedTask;
        }

        [HttpGet("{id}/link")]
        public async Task<IEnumerable<WeekUserLinkDto>> GetLinks(int id)
        {
            var links = await _weekUserLinkService.GetByWeekIdAsync(id);
            return await _weekUserLinkMapper.ToDtoAsync(links);
        }

        [HttpGet("{id}/link/{userId}")]
        public async Task<WeekUserLinkDto> GetLink(int id, int userId)
        {
            var link = await _weekUserLinkService.GetByIdAsync(new WeekUserLinkId {WeekId = id, UserId = userId});
            return await _weekUserLinkMapper.ToDtoAsync(link);
        }

        [HttpPost("{id}/link/{userId}")]
        public async Task<WeekUserLinkDto> UpdateLink(int weekId, int userId, [FromBody]WeekUserLinkDto linkDto)
        {
            linkDto.WeekId = weekId;
            linkDto.UserId = userId;

            var link = await _weekUserLinkMapper.ToModelAsync(linkDto);
            await _weekUserLinkService.UpdateAsync(link);
            return linkDto;
        }
    }
}
