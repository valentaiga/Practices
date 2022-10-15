using GraphQL.Types;

namespace Practices.GraphQL.GraphQL.Models;

public sealed class BookType : ObjectGraphType<Book>
{
    public BookType()
    {
        Field(d => d.Id);
        Field(d => d.Description);
        Field(d => d.Title);
        Field(d => d.CreatedAt);
    }
}