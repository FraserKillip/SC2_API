using System;
using System.Collections.Generic;
using System.Linq;
using SC2_API.DTO;
using SC2_API.Repositories;

namespace SC2_API.Services
{
    public class UserService : IUserService
    {
        IUserRepository _userRepository;
        public UserService(IUserRepository userRespository)
        {
            _userRepository = userRespository;
        }

        public int Count()
        {
            return _userRepository.Count();
        }

        public void Delete(UserDto userDto)
        {
            _userRepository.Delete(userDto.ToModel());
        }

        public void Delete(int id)
        {
            _userRepository.Delete(id);
        }

        public IEnumerable<UserDto> Get()
        {
            return _userRepository.Get().Select(u => u.ToDto());
        }

        public UserDto GetById(int id)
        {
            return _userRepository.GetById(id).ToDto();
        }

        public UserDto Insert(UserDto userDto)
        {
            return _userRepository.Insert(userDto.ToModel()).ToDto();
        }

        public void Update(UserDto userDto)
        {
            _userRepository.Update(userDto.ToModel());
        }
    }
}
