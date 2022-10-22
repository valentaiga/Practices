using GraphQL.Types;
using Practices.GraphQL.Models.Author.Query;
using Practices.GraphQL.Services;

namespace Practices.GraphQL.Models.Book.Query;

public sealed class BookType : ObjectGraphType<Book>
{
    public BookType(IAuthorRepository authorRepository)
    {
        Field(d => d.Id);
        Field(d => d.Description);
        Field(d => d.Title);
        Field(d => d.CreatedAt);
        Field<AuthorType>("author")
            .Description("Author of the book")
            .ResolveAsync(async ctx =>
            {
                var authorId = ctx.Source.AuthorId;
                return await authorRepository.Get(authorId);
            });
    }
}