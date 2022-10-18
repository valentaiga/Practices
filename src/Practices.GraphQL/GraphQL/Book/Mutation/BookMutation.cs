using GraphQL;
using GraphQL.Types;
using Practices.GraphQL.GraphQL.Book.Query;
using Practices.GraphQL.Services;

namespace Practices.GraphQL.GraphQL.Book.Mutation;

public sealed class BookMutation : ObjectGraphType
{
    public BookMutation(IBookRepository bookRepository)
    {
        Name = "Mutation";

        Field<BookType>("createBook")
            .Description("Create book")
            .Arguments(new QueryArgument<BookInputType> { Name = "book" })
            .ResolveAsync(async context =>
            {
                var bookInput = context.GetArgument<Book>("book");
                return await bookRepository.Create(bookInput.Title, bookInput.Description);
            });
    }
}