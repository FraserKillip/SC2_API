using GraphQL.Types;
using SandwichClub.Api.Repositories.Models;
using SandwichClub.Api.Services;

namespace SandwichClub.Api.GraphQL.Types {
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