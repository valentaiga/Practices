using GraphQL.Types;
using Practices.GraphQL.Models.Author;
using Practices.GraphQL.Models.Book;

namespace Practices.GraphQL.Models;

public sealed class StoreQuery : ObjectGraphType
{
    public StoreQuery()
    {
        Name = "Query";
        Field<AuthorGroupType>("author").Resolve(_ => new {});
        Field<BookGroupType>("book").Resolve(_ => new {});
    }
}