using GraphQL.Types;
using SandwichClub.Api.Repositories.Models;
using SandwichClub.Api.Services;

namespace SandwichClub.Api.GraphQL.Types {
    public class WeekUserLinkType : ObjectGraphType
    {
      public WeekUserLinkType(IWeekService weekService, IUserService userService)
      {
        Name = "WeekUserLink";
        Field<NonNullGraphType<IntGraphType>>("WeekId", "The weeks id");
        Field<NonNullGraphType<IntGraphType>>("UserId", "The id of the user.");
        Field<IntGraphType>("Slices", "The slices taken.");
        Field<DecimalGraphType>("Paid", "The amount paid");

        Field<UserType>("User", "The user for the link", resolve: context => userService.GetByIdAsync(((WeekUserLink) context.Source).UserId));
        Field<WeekType>("Week", "The week for the link", resolve: context => weekService.GetByIdAsync(((WeekUserLink) context.Source).WeekId));

        IsTypeOf = value => value is WeekUserLink;
      }
    }
}