using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SandwichClub.Api.Controllers.Mapper;
using SandwichClub.Api.Dto;
using SandwichClub.Api.Repositories.Models;
using SandwichClub.Api.Services;

namespace SandwichClub.Api.Controllers
{
    public class UsersController : ControllerBase<int, User, UserDto, IUserService>
    {
        private readonly IScSession _session;

        public UsersController(IUserService userService, IMapper<User, UserDto> mapper, IScSession session) : base(userService, mapper)
        {
            _session = session;
        }

        [HttpGet("me")]
        public async Task<UserDto> GetMe()
        {
            return await Mapper.ToDtoAsync(_session.CurrentUser);
        }
    }
}
