using GraphQL.Types;
using SandwichClub.Api.Repositories.Models;
using SandwichClub.Api.Services;

namespace SandwichClub.Api.GraphQL.Types
{
    public class WeekType : AutoObjectGraphType<Week>
    {
        public WeekType(IWeekService weekService, IWeekUserLinkService weekUserLinkService, IUserService userService)
        {
            FieldAsync<DecimalGraphType>("costPerUser", "The amount owed per user", resolve: async context => await weekService.GetAmountToPayPerPersonAsync(context.Source.WeekId));

            FieldAsync<ListGraphType<WeekUserLinkType>>("users", resolve: async context => await weekUserLinkService.GetByWeekIdAsync(context.Source.WeekId));
            FieldAsync<UserType>("shopper", resolve: async context => context.Source.ShopperUserId.HasValue ? await userService.GetByIdAsync(context.Source.ShopperUserId.Value) : null);
        }
    }
}