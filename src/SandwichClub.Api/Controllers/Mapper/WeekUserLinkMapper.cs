using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SandwichClub.Api.Dto;
using SandwichClub.Api.Repositories.Models;
using SandwichClub.Api.Services;

namespace SandwichClub.Api.Controllers.Mapper
{
    public class WeekUserLinkMapper : BaseMapper<WeekUserLink, WeekUserLinkDto>
    {
        private readonly IUserService _userService;
        private readonly IMapper<User, UserDto> _userMapper;

        public WeekUserLinkMapper(IUserService userService, IMapper<User, UserDto> userMapper)
        {
            _userService = userService;
            _userMapper = userMapper;
        }

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

        public override async Task<WeekUserLinkDto> ToDtoAsync(WeekUserLink link)
        {
            var user = await _userService.GetByIdAsync(link.UserId);
            var userDto = await _userMapper.ToDtoAsync(user);

            return ToDto(link, userDto);
        }

        public override async Task<IEnumerable<WeekUserLinkDto>> ToDtoAsync(IEnumerable<WeekUserLink> links)
        {
            var dtos = links.Select(l => ToDto(l)).ToList();
            var userIds = dtos.Select(l => l.UserId);

            var users = await _userService.GetByIdsAsync(userIds);
            var userDtos = (await _userMapper.ToDtoAsync(users)).ToDictionary(u => u.Id);

            foreach (var dto in dtos)
            {
                UserDto user;
                if (userDtos.TryGetValue(dto.UserId, out user))
                    dto.User = user;
            }

            return dtos;
        }

        private WeekUserLinkDto ToDto(WeekUserLink link, UserDto userDto = null)
        {
            return new WeekUserLinkDto
            {
                WeekId = link.WeekId,
                UserId = link.UserId,

                Paid = link.Paid,
                Slices = link.Slices,

                User = userDto,
            };
        }
    }
}
