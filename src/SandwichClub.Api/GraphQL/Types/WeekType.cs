using GraphQL.Types;
using SandwichClub.Api.Repositories.Models;
using SandwichClub.Api.Services;

namespace SandwichClub.Api.GraphQL.Types
{
    public class WeekType : ObjectGraphType
    {
        public WeekType(IWeekService weekService, IWeekUserLinkService weekUserLinkService, IUserService userService)
        {
            Name = "week";
            Field<NonNullGraphType<IntGraphType>>("weekId", "The weeks id");
            Field<IntGraphType>("shopperUserId", "THe weeks shopper");
            Field<DecimalGraphType>("cost", "The Cost of the week");

            FieldAsync<DecimalGraphType>("costPerUser", "The amount owed per user", resolve: async context => await weekService.GetAmountToPayPerPersonAsync(((Week)context.Source).WeekId));

            FieldAsync<ListGraphType<WeekUserLinkType>>("users", resolve: async context => await weekUserLinkService.GetByWeekIdAsync(((Week)context.Source).WeekId));
            FieldAsync<UserType>("shopper", resolve: async context => ((Week)context.Source).ShopperUserId.HasValue ? await userService.GetByIdAsync(((Week)context.Source).ShopperUserId.Value) : null);

            IsTypeOf = value => value is Week;
        }
    }
}