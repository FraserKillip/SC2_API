namespace SandwichClub.Api.GraphQL.Middleware
{
    internal sealed class GraphQLRequest
    {
        public string OperationName { get; set; }

        public string Query { get; set; }
        public string Mutation { get; set; }

        public string Variables { get; set; }
    }
}
