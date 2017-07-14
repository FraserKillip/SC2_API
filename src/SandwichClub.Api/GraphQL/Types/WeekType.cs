using GraphQL.Types;
using SandwichClub.Api.Repositories.Models;
using SandwichClub.Api.Services;

namespace SandwichClub.Api.GraphQL.Types
{
    public class WeekType : AutoObjectGraphType<Week>
    {
        public WeekType(IWeekService weekService, IWeekUserLinkService weekUserLinkService, IUserService userService)
        {
            FieldAsync<DecimalGraphType>("costPerUser", "The amount owed per user", resolve: async context => await weekService.GetAmountToPayPerPersonAsync(((Week)context.Source).WeekId));

            FieldAsync<ListGraphType<WeekUserLinkType>>("users", resolve: async context => await weekUserLinkService.GetByWeekIdAsync(((Week)context.Source).WeekId));
            FieldAsync<UserType>("shopper", resolve: async context => ((Week)context.Source).ShopperUserId.HasValue ? await userService.GetByIdAsync(((Week)context.Source).ShopperUserId.Value) : null);
        }
    }
}