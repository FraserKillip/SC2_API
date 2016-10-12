using GraphQL.Types;
using SandwichClub.Api.Repositories;
using SandwichClub.Api.Repositories.Models;
using SandwichClub.Api.Services;

namespace SandwichClub.Api.GraphQL {
    public interface ISandwichClubSchema : IObjectGraphType {}

    public class SandwichClubSchema : ObjectGraphType, ISandwichClubSchema {
        public SandwichClubSchema(IScSession session, IUserRepository userRepository) {
            Name = "Query";
            Field<UserType>(
                "me",
                resolve: context => userRepository.GetByIdAsync(session.CurrentUser.UserId)
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