using GraphQL.Types;

namespace Practices.GraphQL.Web.Models.Author.Mutation;

public sealed class AuthorUpdateType : InputObjectGraphType
{
    public AuthorUpdateType()
    {
        Field<NonNullGraphType<IntGraphType>>("id");
        Field<StringGraphType>("name");
    }
}