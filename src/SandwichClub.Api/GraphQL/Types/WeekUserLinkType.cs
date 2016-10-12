using GraphQL.Types;
using SandwichClub.Api.Repositories.Models;
using SandwichClub.Api.Services;

namespace SandwichClub.Api.GraphQL.Types {
    public class WeekUserLinkType : ObjectGraphType
    {
      public WeekUserLinkType()
      {
        Name = "WeekUserLink";
        Field<NonNullGraphType<IntGraphType>>("WeekId", "The weeks id");
        Field<NonNullGraphType<IntGraphType>>("UserId", "The id of the user.");
        Field<IntGraphType>("Slices", "The slices taken.");
        Field<DecimalGraphType>("Paid", "The amount paid");

        IsTypeOf = value => value is WeekUserLink;
      }
    }
}