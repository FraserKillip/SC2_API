using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SandwichClub.Api.DTO;
using SandwichClub.Api.Services;

namespace SandwichClub.Api.Controllers
{
    [Route("api/[controller]")]
    public class WeeksController : Controller
    {
        private IWeekService _weekService;
        private IWeekUserLinkService _weekUserLinkService;

        public WeeksController(IWeekService weekService, IWeekUserLinkService weekUserLinkService)
        {
            _weekService = weekService;
            _weekUserLinkService = weekUserLinkService;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<WeekDto> Get()
        {
            return _weekService.Get();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public WeekDto Get(int id)
        {
            return _weekService.GetById(id);
        }

        // POST api/values
        [HttpPost]
        public WeekDto Post([FromBody]WeekDto week)
        {
            return _weekService.Insert(week);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public WeekDto Put(int id, [FromBody]WeekDto week)
        {
            return week;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet("{id}/link/")]
        public IEnumerable<WeekUserLinkDto> GetLinks(int id)
        {
            return new WeekUserLinkDto[] { };
        }

        [HttpGet("{id}/link/{linkId}")]
        public WeekUserLinkDto GetLink(int id, int linkId)
        {
            return new WeekUserLinkDto { User = new UserDto { Id = id }, Slices = linkId };
        }

        [HttpPost("{id}/link/")]
        public WeekUserLinkDto UpdateLink(int id, [FromBody]WeekUserLinkDto link)
        {
            return _weekUserLinkService.updateLink(link, id);
        }
    }
}
