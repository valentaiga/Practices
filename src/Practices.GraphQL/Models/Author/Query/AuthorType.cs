using GraphQL.Types;
using Practices.GraphQL.Models.Book.Query;
using Practices.GraphQL.Services;

namespace Practices.GraphQL.Models.Author.Query;

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