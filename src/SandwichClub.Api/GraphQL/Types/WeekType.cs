using GraphQL.Types;
using SandwichClub.Api.Repositories.Models;
using SandwichClub.Api.Services;

namespace SandwichClub.Api.GraphQL.Types {
    public class WeekType : ObjectGraphType
    {
      public WeekType(IWeekUserLinkService weekUserLinkService)
      {
        Name = "Week";
        Field<NonNullGraphType<IntGraphType>>("WeekId", "The weeks id");
        Field<IntGraphType>("ShopperUserId", "THe weeks shopper");
        Field<DecimalGraphType>("Cost", "The Cost of the week");

        Field<ListGraphType<WeekUserLinkType>>("Users", resolve: context =>  weekUserLinkService.GetByWeekIdAsync(((Week) context.Source).WeekId));

        IsTypeOf = value => value is Week;
      }
    }
}