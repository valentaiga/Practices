using GraphQL.Types;
using Practices.GraphQL.Web.Models.Author.Query;
using Practices.GraphQL.Web.Models.Book.Query;

namespace Practices.GraphQL.Web.Models;

public sealed class StoreQuery : ObjectGraphType
{
    public StoreQuery()
    {
        Name = "Query";
        Field<AuthorGroupType>("author").Resolve(_ => new {});
        Field<BookGroupType>("book").Resolve(_ => new {});
    }
}