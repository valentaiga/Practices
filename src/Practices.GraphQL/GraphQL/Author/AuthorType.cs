using GraphQL.Types;

namespace Practices.GraphQL.GraphQL.Author;

public sealed class AuthorType : ObjectGraphType<Author>
{
    public AuthorType()
    {
        Field(d => d.Id);
        Field(d => d.Name);
        Field(d => d.CreatedAt);
    }
}