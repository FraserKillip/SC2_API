using SandwichClub.Api.Repositories.Models;
using SandwichClub.Api.Services;

namespace SandwichClub.Api.GraphQL.Types
{
    public class WeekUserLinkType : AutoObjectGraphType<WeekUserLink>
    {
        public WeekUserLinkType(IWeekService weekService, IUserService userService)
        {
            FieldAsync<UserType>("user", "The user for the link", resolve: async context => await userService.GetByIdAsync(((WeekUserLink)context.Source).UserId));
            FieldAsync<WeekType>("week", "The week for the link", resolve: async context => await weekService.GetByIdAsync(((WeekUserLink)context.Source).WeekId));

            IsTypeOf = value => value is WeekUserLink;
        }
    }
}