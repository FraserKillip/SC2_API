using GraphQL.Types;
using SandwichClub.Api.Repositories.Models;
using SandwichClub.Api.Services;

namespace SandwichClub.Api.GraphQL.Types
{
    public class UserType : AutoObjectGraphType<User>
    {
        public UserType(IWeekService weekService, IWeekUserLinkService weekUserLinkService)
        {
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
        }
    }
}