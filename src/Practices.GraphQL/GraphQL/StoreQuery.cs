using GraphQL.Types;
using Practices.GraphQL.GraphQL.Author;
using Practices.GraphQL.GraphQL.Book;

namespace Practices.GraphQL.GraphQL;

public sealed class StoreQuery : ObjectGraphType
{
    public StoreQuery()
    {
        Name = "Query";
        Field<AuthorGroupType>("author").Resolve(_ => new {});
        Field<BookGroupType>("book").Resolve(_ => new {});
    }
}