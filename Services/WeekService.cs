using System;
using System.Collections.Generic;
using System.Linq;
using SC2_API.DTO;
using SC2_API.Repositories;

namespace SC2_API.Services
{
    public class WeekService : IWeekService
    {
        private IWeekRepository _weekRepository;
        private IWeekUserLinkRepository _weekUserLinkRepository;
        private IUserRepository _userRepository;
        public WeekService(IWeekRepository weekRepository, IWeekUserLinkRepository weekUserLinkRepository, IUserRepository userRepository)
        {
            _weekRepository = weekRepository;
            _weekUserLinkRepository = weekUserLinkRepository;
            _userRepository = userRepository;
        }

        public int Count()
        {
            return _weekRepository.Count();
        }

        public void Delete(WeekDto weekDto)
        {
            _weekRepository.Delete(weekDto.ToModel());
        }

        public void Delete(int id)
        {
            _weekRepository.Delete(id);
        }

        public IEnumerable<WeekDto> Get()
        {
            return _weekRepository.Get().Select(w => w.ToDto()).ToList();
        }

        public WeekDto GetById(int id)
        {
            var weekModel = _weekRepository.GetById(id);

            WeekDto week;

            if (weekModel == null) {

                week = new WeekDto
                {
                    Id = id
                };

                _weekRepository.Insert(week.ToModel());
            } else {
                week = weekModel.ToDto();
            }

            hydrateLinks(week);

            return week;
        }

    public WeekDto Insert(WeekDto weekDto)
    {
        return _weekRepository.Insert(weekDto.ToModel()).ToDto();
    }

    public void Update(WeekDto weekDto)
    {
        _weekRepository.Update(weekDto.ToModel());
    }

    private void hydrateLinks(WeekDto week)
    {
        var links = _weekUserLinkRepository.GetByWeekId(week.Id);
        var users = _userRepository.Get();

        var newLinks = users.Select(u =>
        {
            var link = links.FirstOrDefault(l => l.UserId == u.UserId);
            if (link != null)
            {
                return new WeekUserLinkDto
                {
                    User = u.ToDto(),
                    Paid = link.Paid,
                    Slices = link.Slices
                };
            }

            return new WeekUserLinkDto
            {
                User = u.ToDto()
            };
        });

        week.Links = newLinks;
    }
}
}
