using GraphQL.Types;
using SandwichClub.Api.Repositories.Models;
using SandwichClub.Api.Services;

namespace SandwichClub.Api.GraphQL.Types {
    public class WeekUserLinkType : ObjectGraphType
    {
      public WeekUserLinkType(IWeekService weekService, IUserService userService)
      {
        Name = "weekUserLink";
        Field<NonNullGraphType<IntGraphType>>("weekId", "The weeks id");
        Field<NonNullGraphType<IntGraphType>>("userId", "The id of the user.");
        Field<IntGraphType>("slices", "The slices taken.");
        Field<DecimalGraphType>("paid", "The amount paid");

        Field<UserType>("user", "The user for the link", resolve: context => userService.GetByIdAsync(((WeekUserLink) context.Source).UserId));
        Field<WeekType>("week", "The week for the link", resolve: context => weekService.GetByIdAsync(((WeekUserLink) context.Source).WeekId));

        IsTypeOf = value => value is WeekUserLink;
      }
    }
}