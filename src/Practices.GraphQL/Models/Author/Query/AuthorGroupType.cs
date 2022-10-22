using GraphQL;
using GraphQL.Types;
using Practices.GraphQL.Services;

namespace Practices.GraphQL.Models.Author.Query;

public sealed class AuthorGroupType : ObjectGraphType
{
    public AuthorGroupType(IAuthorRepository authorRepository)
    {
        Field<AuthorType>("author")
            .Description("Query a specific author")
            .Argument<NonNullGraphType<IntGraphType>>("id")
            .ResolveAsync(async context =>
            {
                var id = context.GetArgument<int>("id");
                var author = await authorRepository.Get(id);
                if (author is null) 
                    throw new ExecutionError("Invalid author id");
                return author;
            });
        Field<ListGraphType<AuthorType>>("authors")
            .Description("Query all authors")
            .ResolveAsync(async _ => await authorRepository.GetAll());
    }
}