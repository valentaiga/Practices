using GraphQL.Types;
using Practices.GraphQL.Models.Author.Mutation;
using Practices.GraphQL.Models.Book.Mutation;

namespace Practices.GraphQL.Models;

public sealed class StoreMutation : ObjectGraphType
{
    public StoreMutation()
    {
        Name = "Mutation";
        Field<AuthorGroupMutation>("author").Resolve(_ => new {});
        Field<BookGroupMutation>("book").Resolve(_ => new {});
    }
}