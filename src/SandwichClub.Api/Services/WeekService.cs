using System.Threading.Tasks;
using SandwichClub.Api.DTO;
using SandwichClub.Api.Services.Mapper;
using SandwichClub.Api.Repositories;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Services
{
    public class WeekService : BaseService<int, Week, WeekDto, IWeekRepository>, IWeekService
    {
        private readonly IWeekUserLinkService _weekUserLinkService;

        public WeekService(IWeekRepository weekRepository, IMapper<Week, WeekDto> mapper, IWeekUserLinkService weekUserLinkService) : base(weekRepository, mapper)
        {
            _weekUserLinkService = weekUserLinkService;
        }

        public override async Task<WeekDto> GetByIdAsync(int id)
        {
            var week = await base.GetByIdAsync(id);
            if (week != null) return week;

            // Create default
            week = new WeekDto { Id = id };
            await HydrateDtoAsync(null, week);
            return week;
        }

        protected override async Task HydrateDtoAsync(Week week, WeekDto weekDto)
        {
            // TODO - Get all users
            weekDto.Links = await _weekUserLinkService.GetByWeekIdAsync(weekDto.Id);
        }
    }
}
