using GraphQL;
using GraphQL.Types;
using Practices.GraphQL.Models.Book.Query;
using Practices.GraphQL.Services;

namespace Practices.GraphQL.Models.Book.Mutation;

public sealed class BookGroupMutation : ObjectGraphType
{
    public BookGroupMutation(IBookRepository bookRepository, IAuthorRepository authorRepository, IBookEventService bookEventService)
    {
        Field<BookType>("create")
            .Description("Create book")
            .Arguments(new QueryArgument<BookCreateType> { Name = "book" })
            .ResolveAsync(async ctx =>
            {
                var bookInput = ctx.GetArgument<Book>("book");
                if (!await authorRepository.Exists(bookInput.AuthorId))
                    throw new ExecutionError("Invalid author id");
                return await bookRepository.Create(bookInput.Title, bookInput.Description, bookInput.AuthorId);
            });

        Field<BookType>("update")
            .Description("Update book's fields")
            .Arguments(new QueryArgument<BookUpdateType> { Name = "book" })
            .ResolveAsync(async ctx =>
            {
                var bookInput = ctx.GetArgument<Book>("book");
                return await bookRepository.Update(bookInput.Id, async book =>
                {
                    if (book is null) 
                        throw new ExecutionError("Invalid book id");
                    if (!string.IsNullOrEmpty(bookInput.Title)) book.Title = bookInput.Title;
                    if (!string.IsNullOrWhiteSpace(bookInput.Description)) book.Description = bookInput.Description;
                    if (bookInput.AuthorId != 0)
                    {
                        if (!await authorRepository.Exists(book.AuthorId))
                            throw new ExecutionError("Invalid author id");
                        book.AuthorId = bookInput.AuthorId;
                    }
                    bookEventService.BookChanged(book);
                });
            });

        Field<bool>("delete")
            .Description("Remove book from database")
            .Argument<NonNullGraphType<IntGraphType>>("id")
            .ResolveAsync(async ctx =>
            {
                var id = ctx.GetArgument<int>("id");
                if (!await bookRepository.Exists(id)) 
                    throw new ExecutionError("Invalid book id");
                await bookRepository.Delete(id);
                return true;
            });
    }
}