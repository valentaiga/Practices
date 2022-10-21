using GraphQL.Types;

namespace Practices.GraphQL.Models.Author;

public sealed class AuthorCreateType: InputObjectGraphType
{
    public AuthorCreateType()
    {
        Field<NonNullGraphType<StringGraphType>>("name");
    }
}