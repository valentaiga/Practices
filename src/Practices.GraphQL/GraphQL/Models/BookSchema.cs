using GraphQL.Types;

namespace Practices.GraphQL.GraphQL.Models;

public class BookSchema : Schema
{
    public BookSchema(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
        Query = serviceProvider.GetRequiredService<BookQuery>();
    }
}