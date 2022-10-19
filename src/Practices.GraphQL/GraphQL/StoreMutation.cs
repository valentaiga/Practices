using GraphQL.Types;
using Practices.GraphQL.GraphQL.Author;
using Practices.GraphQL.GraphQL.Book;

namespace Practices.GraphQL.GraphQL;

public sealed class StoreMutation : ObjectGraphType
{
    public StoreMutation()
    {
        Name = "Mutation";
        // Field<AuthorGroupType>("author").Resolve(_ => new {});
        // Field<BookGroupType>("book").Resolve(_ => new {});
        Field<AuthorGroupMutation>("author").Resolve(_ => new {});
        Field<BookGroupMutation>("book").Resolve(_ => new {});
    }
}