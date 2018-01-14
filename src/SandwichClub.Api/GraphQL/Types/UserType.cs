using GraphQL.Types;
using SandwichClub.Api.Repositories.Models;
using SandwichClub.Api.Services;

namespace SandwichClub.Api.GraphQL.Types
{
    public class UserType : AutoObjectGraphType<User>
    {
        public UserType(IWeekService weekService,
            IWeekUserLinkService weekUserLinkService,
            IPaymentService paymentService)
        {
            FieldAsync<DecimalGraphType>("totalCost", "The sum of all week costs for weeks the user is signed up to", resolve: async context => await weekService.GetTotalCostsForUserAsync(context.Source.UserId));
            FieldAsync<DecimalGraphType>("totalPaid", "The sum of amounts paid for all weeks", resolve: async context => await paymentService.GetTotalPaidForUser(context.Source.UserId));

            FieldAsync<DecimalGraphType>("totalOwed", "The total amount owed by the user", resolve: async context => await paymentService.GetTotalOwedForUser(context.Source.UserId));

            FieldAsync<ListGraphType<PaymentType>>("payments", "The users payments",
                resolve: async context => await paymentService.GetByUserIdAsync(context.Source.UserId));

            FieldAsync<ListGraphType<WeekUserLinkType>>("weeks", "The users joined weeks",
                arguments: new QueryArguments(
                    new QueryArgument<BooleanGraphType> { Name = "unpaidOnly", Description = "Include only unpaid weeks" }
                ),
                resolve: async context =>
                {
                    var unpaidOnly = context.GetArgument<bool?>("unpaidOnly");
                    if (unpaidOnly.HasValue && unpaidOnly.Value)
                    {
                        return await weekUserLinkService.GetByUserIdAsync(context.Source.UserId, true);
                    }
                    return await weekUserLinkService.GetByUserIdAsync(context.Source.UserId);
                });
        }
    }
}