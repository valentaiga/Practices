using GraphQL;
using GraphQL.Types;
using Practices.GraphQL.Services;

namespace Practices.GraphQL.Models.Book;

public sealed class BookGroupMutation : ObjectGraphType
{
    public BookGroupMutation(IBookRepository bookRepository)
    {
        Field<BookType>("create")
            .Description("Create book")
            .Arguments(new QueryArgument<BookCreateType> { Name = "book" })
            .ResolveAsync(async ctx =>
            {
                var bookInput = ctx.GetArgument<Book>("book");
                return await bookRepository.Create(bookInput.Title, bookInput.Description, bookInput.AuthorId);
            });

        Field<BookType>("update")
            .Description("Update book's fields")
            .Arguments(new QueryArgument<BookUpdateType> { Name = "book" })
            .ResolveAsync(async ctx =>
            {
                var bookInput = ctx.GetArgument<Book>("book");
                return await bookRepository.Update(bookInput.Id, book =>
                {
                    if (!string.IsNullOrEmpty(bookInput.Title)) book.Title = bookInput.Title;
                    if (!string.IsNullOrWhiteSpace(bookInput.Description)) book.Description = bookInput.Description;
                    if (bookInput.AuthorId != 0) book.AuthorId = bookInput.AuthorId;
                });
            });

        Field<bool>("delete")
            .Description("Remove book from database")
            .Argument<NonNullGraphType<IntGraphType>>("id")
            .ResolveAsync(async ctx =>
            {
                var id = ctx.GetArgument<int>("id");
                await bookRepository.Delete(id);
                return true;
            });
    }
}