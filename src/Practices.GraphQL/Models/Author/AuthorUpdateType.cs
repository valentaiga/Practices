using GraphQL.Types;

namespace Practices.GraphQL.Models.Author;

public sealed class AuthorUpdateType : InputObjectGraphType
{
    public AuthorUpdateType()
    {
        Field<NonNullGraphType<IntGraphType>>("id");
        Field<StringGraphType>("name");
    }
}