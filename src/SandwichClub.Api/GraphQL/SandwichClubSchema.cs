using System;
using GraphQL.Types;

namespace SandwichClub.Api.GraphQL
{
    public class SandwichClubSchema : Schema
    {
        public SandwichClubSchema(Func<Type, GraphType> resolveType)
            : base(resolveType)
        {
            Query = (SandwichClubQuery)resolveType(typeof (SandwichClubQuery));
            Mutation = (SandwichClubMutation)resolveType(typeof (SandwichClubMutation));
        }
    }
}