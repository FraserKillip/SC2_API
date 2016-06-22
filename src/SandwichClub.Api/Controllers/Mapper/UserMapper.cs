using System.Threading.Tasks;
using SandwichClub.Api.DTO;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Controllers.Mapper
{
    public class UserMapper : BaseMapper<User, UserDto>
    {
        public override Task<User> ToModelAsync(UserDto user)
        {
            return Task.FromResult(new User
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
            });
        }

        public override Task<UserDto> ToDtoAsync(User user)
        {
            return Task.FromResult(new UserDto
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
            });
        }
    }
}
