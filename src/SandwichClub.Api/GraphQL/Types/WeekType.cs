using GraphQL.Types;
using SandwichClub.Api.Repositories.Models;
using SandwichClub.Api.Services;

namespace SandwichClub.Api.GraphQL.Types {
    public class WeekType : ObjectGraphType
    {
        public WeekType(IWeekService weekService, IWeekUserLinkService weekUserLinkService, IUserService userService)
        {
            Name = "week";
            Field<NonNullGraphType<IntGraphType>>("weekId", "The weeks id");
            Field<IntGraphType>("shopperUserId", "THe weeks shopper");
            Field<DecimalGraphType>("cost", "The Cost of the week");

            Field<DecimalGraphType>("costPerUser", "The amount owed per user", resolve: context => weekService.GetAmountToPayPerPersonAsync(((Week) context.Source).WeekId));

            Field<ListGraphType<WeekUserLinkType>>("users", resolve: context =>  weekUserLinkService.GetByWeekIdAsync(((Week) context.Source).WeekId));
            Field<UserType>("shopper", resolve: context =>  ((Week) context.Source).ShopperUserId.HasValue ? userService.GetByIdAsync(((Week) context.Source).ShopperUserId.Value) : null);

            IsTypeOf = value => value is Week;
        }
    }
}