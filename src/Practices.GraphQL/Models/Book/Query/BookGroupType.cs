using GraphQL;
using GraphQL.Types;
using Practices.GraphQL.Services;

namespace Practices.GraphQL.Models.Book.Query;

public sealed class BookGroupType : ObjectGraphType
{
    public BookGroupType(IBookRepository bookRepository)
    {
        Field<BookType>("book")
            .Description("Query a specific book")
            .Argument<NonNullGraphType<IntGraphType>>("id")
            .ResolveAsync(async context =>
            {
                var id = context.GetArgument<int>("id");
                var book = await bookRepository.Get(id);
                if (book is null) 
                    throw new ExecutionError("Invalid book id");
                return book;
            });
        Field<ListGraphType<BookType>>("books")
            .Description("Query all books")
            .ResolveAsync(async _ => await bookRepository.GetAll());
    }
}