using GraphQL.Types;
using Practices.GraphQL.GraphQL.Book;
using Practices.GraphQL.Services;

namespace Practices.GraphQL.GraphQL.Author;

public sealed class AuthorType : ObjectGraphType<Author>
{
    public AuthorType(IBookRepository bookRepository)
    {
        Field(d => d.Id);
        Field(d => d.Name);
        Field(d => d.CreatedAt);
        Field<ListGraphType<BookType>>("books")
            .Description("Books written by author")
            .ResolveAsync(async ctx =>
            {
                var authorId = ctx.Source.Id;
                return await bookRepository.GetByAuthor(authorId);
            });
    }
}