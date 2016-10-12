using GraphQL.Types;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.GraphQL {
    public class SandwichClubSchema : ObjectGraphType {
        public SandwichClubSchema() {
            Name = "Query";
            Field<UserType>(
                "user",
                resolve: context => new User { UserId = 1, FirstName = "Test" }
            );
        }
    }

    public class UserType : ObjectGraphType
    {
      public UserType()
      {
        Name = "User";
        Field<NonNullGraphType<IntGraphType>>("UserId", "The id of the user.");
        Field<StringGraphType>("FirstName", "The First Name of the user");
        IsTypeOf = value => value is User;
      }
    }
}