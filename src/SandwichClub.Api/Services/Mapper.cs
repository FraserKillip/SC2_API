using SandwichClub.Api.DTO;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Services
{
    public static class Mapper
    {
        public static WeekDto ToDto(this Week week) {
            return new WeekDto {
                Id = week.WeekId,
                Shopper = week.ShopperUserId,
                Cost = week.Cost
            };
        }

        public static Week ToModel(this WeekDto week) {
            return new Week {
                WeekId = week.Id,
                ShopperUserId = week.Shopper,
                Cost = week.Cost
            };
        }

        public static UserDto ToDto(this User user) {
            return new UserDto {
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

        public static User ToModel(this UserDto user) {
            return new User {
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

        public static WeekUserLink ToModel(this WeekUserLinkDto link, int weekId) {
            return new WeekUserLink {
                UserId = link.User.Id,
                Paid = link.Paid,
                Slices = link.Slices,
                WeekId = weekId
            };
        }

        public static WeekUserLinkDto ToDto(this WeekUserLink link, IUserService userService) {
            return new WeekUserLinkDto {
                Paid = link.Paid,
                Slices = link.Slices,
                User = userService.GetById(link.UserId)
            };
        }
    }
}