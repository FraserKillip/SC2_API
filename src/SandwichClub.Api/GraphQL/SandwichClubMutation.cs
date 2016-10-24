using GraphQL.Types;
using SandwichClub.Api.GraphQL.Types;
using SandwichClub.Api.Services;

namespace SandwichClub.Api.GraphQL {
    public class SandwichClubMutation : ObjectGraphType {
        public SandwichClubMutation(IScSession session, IUserService userService, IWeekService weekService) {

            Name = "Mutation";
            Field<UserType>(
                "me",
                resolve: context => userService.GetByIdAsync(session.CurrentUser.UserId)
            );
        }
    }
}