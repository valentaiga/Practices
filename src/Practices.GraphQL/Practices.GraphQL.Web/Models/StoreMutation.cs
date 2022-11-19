using GraphQL.Types;
using Practices.GraphQL.Web.Models.Author.Mutation;
using Practices.GraphQL.Web.Models.Book.Mutation;

namespace Practices.GraphQL.Web.Models;

public sealed class StoreMutation : ObjectGraphType
{
    public StoreMutation()
    {
        Name = "Mutation";
        Field<AuthorGroupMutation>("author").Resolve(_ => new {});
        Field<BookGroupMutation>("book").Resolve(_ => new {});
    }
}