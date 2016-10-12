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
            Field<ListGraphType<UserType>>(
                "users",
                resolve: context => userRepository.GetAsync()
            );
            Field<UserType>(
                "user",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "UserId", Description = "UserId of the user" },
                    new QueryArgument<StringGraphType> { Name = "FacebookId", Description = "FacebookId of the user" }
                ),
                resolve: context => {
                    var userId = context.GetArgument<int?>("UserId");
                    if (userId != null) return userRepository.GetByIdAsync(userId.Value);
                    var socialId = context.GetArgument<string>("FacebookId");
                    if (socialId != null) return userRepository.GetBySocialId(socialId);
                    return null;
                }
            );
        }
    }

    public class UserType : ObjectGraphType
    {
      public UserType()
      {
        Name = "User";
        Field<NonNullGraphType<IntGraphType>>("UserId", "The id of the user.");
        Field<StringGraphType>("FacebookId", "The FacebookId of the user");
        Field<StringGraphType>("FirstName", "The First Name of the user");
        Field<StringGraphType>("LastName", "The First Last of the user");
        Field<StringGraphType>("Email", "The Email of the user");
        Field<StringGraphType>("AvatarUrl", "The AvatarUrl of the user");
        Field<BooleanGraphType>("Shopper", "Whether the user is valid for shopping or not");
        Field<StringGraphType>("BankDetails", "The BankDetails of the user");
        Field<StringGraphType>("PhoneNumber", "The PhoneNumber of the user");
        Field<StringGraphType>("BankName", "The BankName of the user");
        Field<BooleanGraphType>("FirstLogin", "Whether the user is logging in for the first time");
        IsTypeOf = value => value is User;
      }
    }
}