using GraphQL;
using GraphQL.Types;
using Practices.GraphQL.Services;

namespace Practices.GraphQL.Models.Author;

public sealed class AuthorGroupMutation : ObjectGraphType
{
    public AuthorGroupMutation(IAuthorRepository authorRepository)
    {
        Field<AuthorType>("create")
            .Description("Create author")
            .Arguments(new QueryArgument<AuthorCreateType> { Name = "author" })
            .ResolveAsync(async context =>
            {
                var author = context.GetArgument<Author>("author");
                return await authorRepository.Create(author.Name);
            });
        
        Field<AuthorType>("update")
            .Description("Update author's fields")
            .Arguments(new QueryArgument<AuthorUpdateType> { Name = "author" })
            .ResolveAsync(async ctx =>
            {
                var authorInput = ctx.GetArgument<Author>("author");
                return await authorRepository.Update(authorInput.Id, author =>
                {
                    if (!string.IsNullOrEmpty(authorInput.Name)) author.Name = authorInput.Name;
                });
            });

        Field<bool>("delete")
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