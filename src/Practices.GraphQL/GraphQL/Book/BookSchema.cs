using GraphQL.Types;
using Practices.GraphQL.GraphQL.Book.Mutation;
using Practices.GraphQL.GraphQL.Book.Query;

namespace Practices.GraphQL.GraphQL.Book;

public class BookSchema : Schema
{
    public BookSchema(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
        Query = serviceProvider.GetRequiredService<BookQuery>();
        Mutation = serviceProvider.GetRequiredService<BookMutation>();
    }
}