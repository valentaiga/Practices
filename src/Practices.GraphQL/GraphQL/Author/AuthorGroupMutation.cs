using GraphQL;
using GraphQL.Types;
using Practices.GraphQL.Services;

namespace Practices.GraphQL.GraphQL.Author;

public sealed class AuthorGroupMutation : ObjectGraphType
{
    public AuthorGroupMutation(IAuthorRepository authorRepository)
    {
        Field<AuthorType>("createAuthor")
            .Description("Create author")
            .Arguments(new QueryArgument<AuthorInputType> { Name = "author" })
            .ResolveAsync(async context =>
            {
                var author = context.GetArgument<Author>("author");
                return await authorRepository.Create(author.Name);
            });

        Field<bool>("deleteAuthor")
            .Description("Remove author from database")
            .Argument<NonNullGraphType<IntGraphType>>("id")
            .ResolveAsync(async context =>
            {
                var id = context.GetArgument<int>("id");
                await authorRepository.Delete(id);
                return true;
            });
    }
}