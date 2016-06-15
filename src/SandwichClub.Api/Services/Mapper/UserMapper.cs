using SandwichClub.Api.DTO;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Services.Mapper
{
    public class UserMapper : BaseMapper<User, UserDto>
    {
        public override User ToModel(UserDto user)
        {
            return new User
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                AvatarUrl = user.AvatarUrl,
                Shopper = user.Shopper,
                BankDetails = user.BankDetails,
                PhoneNumber = user.PhoneNumber,
                BankName = user.BankName,
                FirstLogin = user.FirstLogin
            };
        }

        public override UserDto ToDto(User user)
        {
            return new UserDto
            {
                Id = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                AvatarUrl = user.AvatarUrl,
                Shopper = user.Shopper,
                BankDetails = user.BankDetails,
                PhoneNumber = user.PhoneNumber,
                BankName = user.BankName,
                FirstLogin = user.FirstLogin
            };
        }
    }
}
