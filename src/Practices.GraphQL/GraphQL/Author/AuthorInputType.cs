using GraphQL.Types;

namespace Practices.GraphQL.GraphQL.Author;

public sealed class AuthorInputType: InputObjectGraphType
{
    public AuthorInputType()
    {
        Field<NonNullGraphType<StringGraphType>>("name");
    }
}