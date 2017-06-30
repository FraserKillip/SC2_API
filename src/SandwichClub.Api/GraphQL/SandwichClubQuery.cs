using GraphQL.Types;
using SandwichClub.Api.GraphQL.Types;
using SandwichClub.Api.Repositories.Models;
using SandwichClub.Api.Services;

namespace SandwichClub.Api.GraphQL {
    public class SandwichClubQuery : ObjectGraphType {
        public SandwichClubQuery(IScSession session, IUserService userService, IWeekService weekService, IWeekUserLinkService weekUserLinkService) {

            Name = "Query";
            FieldAsync<UserType>(
                "me",
                resolve: async context => {
                    return await userService.GetByIdAsync((session.CurrentUser?.UserId).Value);
                }
            );


            FieldAsync<UserType>(
                "primaryShopper",
                resolve: async context =>
                {
                    return await userService.GetPrimaryShopperAsync();
                }
            );

            FieldAsync<ListGraphType<UserType>>(
                "users",
                resolve: async context => await userService.GetAsync()
            );

            FieldAsync<UserType>(
                "user",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "userId", Description = "UserId of the user" },
                    new QueryArgument<StringGraphType> { Name = "facebookId", Description = "FacebookId of the user" }
                ),
                resolve: async context =>
                {
                    var userId = context.GetArgument<int?>("userId");
                    if (userId != null) return await userService.GetByIdAsync(userId.Value);
                    var socialId = context.GetArgument<string>("facebookId");
                    if (socialId != null) return await userService.GetBySocialId(socialId);
                    return null;
                }
            );

            FieldAsync<WeekType>(
                "thisweek",
                resolve: async context => await weekService.GetCurrentWeekAsync()
            );

            FieldAsync<ListGraphType<WeekType>>(
                "weeks",
                resolve: async context => await weekService.GetAsync()
            );

            FieldAsync<WeekType>(
                "week",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "weekId", Description = "WeekId of the user" }
                ),
                resolve: async context => {
                    var weekId = context.GetArgument<int?>("weekId");
                    if (weekId != null) return await weekService.GetByIdAsync(weekId.Value);
                    return null;
                }
            );

            FieldAsync<WeekUserLinkType>(
                "weekLink",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "weekId", Description = "WeekId of the user" }
                ),
                resolve: async context => {
                    var weekId = context.GetArgument<int>("weekId");
                    return await weekUserLinkService.GetByIdAsync(new WeekUserLinkId { WeekId = weekId, UserId = (session.CurrentUser?.UserId).Value});
                }
            );
        }
    }
}