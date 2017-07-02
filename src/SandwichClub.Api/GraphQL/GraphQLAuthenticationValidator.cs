using GraphQL.Language.AST;
using GraphQL.Validation;
using SandwichClub.Api.Services;
using System.Linq;

namespace SandwichClub.Api.GraphQL
{
    public class GraphQLAuthenticationValidator : IGraphQLAuthenticationValidator
    {
        public INodeVisitor Validate(ValidationContext context)
        {
            var session = context.UserContext as IScSession;

            return new EnterLeaveListener(_ =>
            {
                _.Match<Operation>(op =>
                {
                    // User is authenticated
                    if (session.CurrentUser != null)
                        return;

                    // Allow internal field access
                    if (op.SelectionSet.Selections.All(selection => (selection as Field)?.Name.StartsWith("__") ?? false))
                        return;

                    // No auth & trying to access authenticated field
                    context.ReportError(new ValidationError(
                        context.OriginalQuery,
                        "auth-required",
                        session.InvalidToken ? "Invalid Sandwich-Auth-Token header" : "Missing Sandwich-Auth-Token header"));
                });
            });
        }
    }
}
