using SandwichClub.Api.DTO;
using SandwichClub.Api.Repositories;

namespace SandwichClub.Api.Services
{
    public class WeekUserLinkService : IWeekUserLinkService
    {
        private IWeekRepository _weekRepository;
        private IWeekUserLinkRepository _weekUserLinkRepository;
        private IUserService _userService;
        public WeekUserLinkService(IWeekRepository weekRepository, IWeekUserLinkRepository weekUserLinkRepository, IUserService userService)
        {
            _weekRepository = weekRepository;
            _weekUserLinkRepository = weekUserLinkRepository;
            _userService = userService;
        }

        public WeekUserLinkDto updateLink(WeekUserLinkDto link, int weekId) {
            var model = link.ToModel(weekId);

            var existing = _weekUserLinkRepository.GetByUserAndWeek(model.UserId, model.WeekId);

            if(existing != null)
                _weekUserLinkRepository.Update(model);
            else
                _weekUserLinkRepository.Insert(model);

            return model.ToDto(_userService);
        }
    }
}