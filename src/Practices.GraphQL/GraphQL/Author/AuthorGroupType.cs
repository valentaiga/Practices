using GraphQL;
using GraphQL.Types;
using Practices.GraphQL.Services;

namespace Practices.GraphQL.GraphQL.Author;

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
                return await authorRepository.Get(id);
            });
        Field<ListGraphType<AuthorType>>("authors")
            .Description("Query all authors")
            .ResolveAsync(async _ => await authorRepository.GetAll());
    }
}