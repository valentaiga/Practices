using GraphQL;
using GraphQL.Types;
using Practices.GraphQL.Services;

namespace Practices.GraphQL.GraphQL.Book;

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
                return await bookRepository.Get(id);
            });
        Field<ListGraphType<BookType>>("books")
            .Description("Query all books")
            .ResolveAsync(async _ => await bookRepository.GetAll());
    }
}