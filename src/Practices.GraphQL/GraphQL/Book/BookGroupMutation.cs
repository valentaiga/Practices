using GraphQL;
using GraphQL.Types;
using Practices.GraphQL.Services;

namespace Practices.GraphQL.GraphQL.Book;

public sealed class BookGroupMutation : ObjectGraphType
{
    public BookGroupMutation(IBookRepository bookRepository)
    {
        Field<BookType>("createBook")
            .Description("Create book")
            .Arguments(new QueryArgument<BookInputType> { Name = "book" })
            .ResolveAsync(async context =>
            {
                var bookInput = context.GetArgument<Book>("book");
                return await bookRepository.Create(bookInput.Title, bookInput.Description, bookInput.AuthorId);
            });

        Field<bool>("deleteBook")
            .Description("Remove book from database")
            .Argument<NonNullGraphType<IntGraphType>>("id")
            .ResolveAsync(async context =>
            {
                var id = context.GetArgument<int>("id");
                await bookRepository.Delete(id);
                return true;
            });
    }
}