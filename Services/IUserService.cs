using System.Collections.Generic;
using SC2_API.DTO;

namespace SC2_API.Services
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
