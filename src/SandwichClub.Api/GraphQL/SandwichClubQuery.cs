using GraphQL.Types;
using SandwichClub.Api.GraphQL.Types;
using SandwichClub.Api.Services;

namespace SandwichClub.Api.GraphQL {
    public class SandwichClubQuery : ObjectGraphType {
        public SandwichClubQuery(IScSession session, IUserService userService, IWeekService weekService) {

            Name = "Query";
            Field<UserType>(
                "me",
                resolve: context => userService.GetByIdAsync(session.CurrentUser.UserId)
            );
            Field<ListGraphType<UserType>>(
                "users",
                resolve: context => userService.GetAsync()
            );
            Field<UserType>(
                "user",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "UserId", Description = "UserId of the user" },
                    new QueryArgument<StringGraphType> { Name = "FacebookId", Description = "FacebookId of the user" }
                ),
                resolve: context => {
                    var userId = context.GetArgument<int?>("UserId");
                    if (userId != null) return userService.GetByIdAsync(userId.Value);
                    var socialId = context.GetArgument<string>("FacebookId");
                    if (socialId != null) return userService.GetBySocialId(socialId);
                    return null;
                }
            );

            Field<WeekType>(
                "thisweek",
                resolve: context => weekService.GetCurrentWeekAsync()
            );
        }
    }
}