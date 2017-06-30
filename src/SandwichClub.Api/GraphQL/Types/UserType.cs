using GraphQL.Types;
using SandwichClub.Api.Repositories.Models;
using SandwichClub.Api.Services;

namespace SandwichClub.Api.GraphQL.Types {
    public class UserType : ObjectGraphType
    {
        public UserType(IWeekService weekService, IWeekUserLinkService weekUserLinkService)
        {
            Name = "user";
            Field<NonNullGraphType<IntGraphType>>("userId", "The id of the user.");
            Field<StringGraphType>("facebookId", "The FacebookId of the user");
            Field<StringGraphType>("firstName", "The First Name of the user");
            Field<StringGraphType>("lastName", "The First Last of the user");
            Field<StringGraphType>("email", "The Email of the user");
            Field<StringGraphType>("avatarUrl", "The AvatarUrl of the user");
            Field<BooleanGraphType>("shopper", "Whether the user is valid for shopping or not");
            Field<StringGraphType>("bankDetails", "The BankDetails of the user");
            Field<StringGraphType>("phoneNumber", "The PhoneNumber of the user");
            Field<StringGraphType>("bankName", "The BankName of the user");
            Field<BooleanGraphType>("firstLogin", "Whether the user is logging in for the first time");

            FieldAsync<DecimalGraphType>("totalCost", "The sum of all week costs for weeks the user is signed up to", resolve: async context => await weekService.GetTotalCostsForUserAsync(((User)context.Source).UserId));
            FieldAsync<DecimalGraphType>("totalPaid", "The sum of amounts paid for all weeks", resolve: async context => await weekUserLinkService.GetSumPaidForUserAsync(((User)context.Source).UserId));

            FieldAsync<ListGraphType<WeekUserLinkType>>("weeks", "The users joined weeks",
                arguments: new QueryArguments(
                    new QueryArgument<BooleanGraphType> { Name = "unpaidOnly", Description = "Include only unpaid weeks" }
                ),
                resolve: async context =>
                {
                    var unpaidOnly = context.GetArgument<bool?>("unpaidOnly");
                    if (unpaidOnly.HasValue && unpaidOnly.Value)
                    {
                        return await weekUserLinkService.GetByUserIdAsync(((User)context.Source).UserId, true);
                    }
                    return await weekUserLinkService.GetByUserIdAsync(((User)context.Source).UserId);
                });
            IsTypeOf = value => value is User;
        }
    }
}