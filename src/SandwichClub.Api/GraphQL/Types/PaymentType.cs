using SandwichClub.Api.Repositories.Models;
using SandwichClub.Api.Services;

namespace SandwichClub.Api.GraphQL.Types
{
    public class PaymentType : AutoObjectGraphType<Payment>
    {
        public PaymentType(IUserService userService)
        {
            FieldAsync<UserType>("user",
                resolve: async context => await userService.GetByIdAsync(context.Source.UserId));
        }
    }
}
