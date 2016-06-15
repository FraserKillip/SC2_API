using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SandwichClub.Api.DTO;
using SandwichClub.Api.Repositories.Models;
using SandwichClub.Api.Services;

namespace SandwichClub.Api.Controllers
{
    [Route("api/[controller]")]
    public class WeeksController : ControllerBase<int, WeekDto, IWeekService>
    {
        private IWeekService _weekService;
        private IWeekUserLinkService _weekUserLinkService;

        public WeeksController(IWeekService weekService, IWeekUserLinkService weekUserLinkService) : base(weekService)
        {
            _weekService = weekService;
            _weekUserLinkService = weekUserLinkService;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public override Task Delete(int id)
        {
            return Task.CompletedTask;
        }

        [HttpGet("{id}/link/")]
        public Task<IEnumerable<WeekUserLinkDto>> GetLinks(int id)
        {
            return _weekUserLinkService.GetByWeekIdAsync(id);
        }

        [HttpGet("{id}/link/{userId}")]
        public Task<WeekUserLinkDto> GetLink(int id, int userId)
        {
            return _weekUserLinkService.GetByIdAsync(new WeekUserLinkId {WeekId = id, UserId = userId});
        }

        [HttpPost("{id}/link/{userId}")]
        public async Task<WeekUserLinkDto> UpdateLink(int weekId, int userId, [FromBody]WeekUserLinkDto link)
        {
            link.WeekId = weekId;
            link.UserId = userId;

            await _weekUserLinkService.UpdateAsync(link);
            return link;
        }
    }
}
