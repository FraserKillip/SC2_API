using SandwichClub.Api.Controllers.Mapper;
using SandwichClub.Api.Dto;
using SandwichClub.Api.Repositories.Models;
using SandwichClub.Api.Services;

namespace SandwichClub.Api.Controllers
{
    public class UsersController : ControllerBase<int, User, UserDto, IUserService>
    {
        public UsersController(IUserService userService, IMapper<User, UserDto> mapper) : base(userService, mapper)
        {
        }
    }
}
