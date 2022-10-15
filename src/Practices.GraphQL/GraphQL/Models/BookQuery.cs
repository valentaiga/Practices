using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Types;
using Practices.GraphQL.Services;

namespace Practices.GraphQL.GraphQL.Models;

public sealed class BookQuery : ObjectGraphType
{
    public BookQuery(IBookRepository bookRepository)
    {
        Name = "Query";

        Field<BookType>("book")
            .Description("Query a specific book")
            .Argument<NonNullGraphType<IntGraphType>>("id")
            .ResolveAsync(async context =>
            {
                var id = context.GetArgument<int>("id");
                return await bookRepository.Get(id);
            });
        // Field(
        //     "book",
        //     (obj) => new Book(1, null, null, null), 
        //     true, 
        //     typeof(BookType));
        
        // Field<BookType>("book", 
        //     "Query a specific book",
        //     new QueryArguments(new QueryArgument<IntGraphType>() { Name = "id"}),
        //     context =>
        //     {
        //         var id = context.GetArgument<int>("id");
        //         // todo: solve Task resolve 
        //         return bookRepository.Get(id).Result;
        //     });
    }
}