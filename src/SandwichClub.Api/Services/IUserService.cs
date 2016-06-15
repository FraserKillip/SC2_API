using System.Collections.Generic;
using SandwichClub.Api.DTO;

namespace SandwichClub.Api.Services
{
    public interface IUserService
    {
        UserDto GetById(int id);

        IEnumerable<UserDto> Get();

        int Count();

        UserDto Insert(UserDto UserDto);

        void Update(UserDto UserDto);

        void Delete(int id);

        void Delete(UserDto UserDto);
    }
}
