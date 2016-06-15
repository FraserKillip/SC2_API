using SandwichClub.Api.DTO;
using SandwichClub.Api.Services.Mapper;
using SandwichClub.Api.Repositories;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Services
{
    public class UserService : BaseService<int, User, UserDto, IUserRepository>, IUserService
    {
        public UserService(IUserRepository userRespository, IMapper<User, UserDto> mapper) : base(userRespository, mapper)
        {
        }
    }
}
