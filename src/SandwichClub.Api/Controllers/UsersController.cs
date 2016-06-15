using Microsoft.AspNetCore.Mvc;
using SandwichClub.Api.DTO;
using SandwichClub.Api.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SandwichClub.Api.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : ControllerBase<int, UserDto, IUserService>
    {
        public UsersController(IUserService userService) : base(userService)
        {
        }
    }
}
