using GraphQL.Types;
using SandwichClub.Api.GraphQL.Types;
using SandwichClub.Api.Repositories.Models;
using SandwichClub.Api.Services;

namespace SandwichClub.Api.GraphQL
{
    public class SandwichClubMutation : ObjectGraphType
    {
        public SandwichClubMutation(IScSession session, IUserService userService, IWeekService weekService, ITelemetryService telemetryService)
        {

            Name = "Mutation";
            FieldAsync<WeekType>(
                "subscribeToWeek",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "userId", Description = "UserId of the user" },
                    new QueryArgument<IntGraphType> { Name = "weekId", Description = "WeekId of the week" },
                    new QueryArgument<IntGraphType> { Name = "slices", Description = "WeekId of the week" }
                ),
                resolve: async context =>
                {
                    var userId = context.GetArgument<int>("userId");
                    var weekId = context.GetArgument<int>("weekId");
                    var slices = context.GetArgument<int>("slices");

                    telemetryService.TrackEvent(slices > 0 ? "weekSubscription" : "weekUnsubscription",
                        new {userId, weekId});

                    return await weekService.SubscibeToWeek(weekId, userId, slices);
                }
            );

            FieldAsync<ListGraphType<WeekUserLinkType>>(
                "markAllWeeksPaidForUser",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "userId", Description = "UserId to mark weeks paid for" }
                ),
                resolve: async context =>
                {
                    var userId = context.GetArgument<int>("userId");

                    return await weekService.MarkAllLinksAsPaidForUserAsync(userId);
                }
            );

            FieldAsync<WeekType>(
                "updateWeek",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "weekId", Description = "WeekId of the week" },
                    new QueryArgument<IntGraphType> { Name = "shopperId", Description = "UserId of the shopper" },
                    new QueryArgument<FloatGraphType> { Name = "cost", Description = "cost of the week" }
                ),
                resolve: async context =>
                {
                    var shopperId = context.GetArgument<int?>("shopperId");
                    var weekId = context.GetArgument<int>("weekId");
                    var cost = context.GetArgument<float?>("cost");

                    return await weekService.SaveAsync(new Week
                    {
                        WeekId = weekId,
                        ShopperUserId = shopperId,
                        Cost = cost ?? 0,
                    });
                }
            );

            Field<UserType>(
                "updateMe",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "bankName", Description = "The name of the users week bank" },
                    new QueryArgument<StringGraphType> { Name = "bankDetails", Description = "Details for making payments" },
                    new QueryArgument<BooleanGraphType> { Name = "shopper", Description = "Set as shopper" }
                ),
                resolve: (context) =>
                {
                    var user = session.CurrentUser;

                    var bankName = context.GetArgument<string>("bankName");
                    var bankDetails = context.GetArgument<string>("bankDetails");
                    var shopper = context.GetArgument<bool?>("shopper");

                    if (bankName != null)
                        user.BankName = bankName;
                    if (bankDetails != null)
                        user.BankDetails = bankDetails;
                    if (shopper != null)
                        user.Shopper = shopper.Value;

                    return userService.SaveAsync(user);
                }
            );
        }
    }
}