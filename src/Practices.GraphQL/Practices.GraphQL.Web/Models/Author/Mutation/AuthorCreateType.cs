using GraphQL.Types;

namespace Practices.GraphQL.Web.Models.Author.Mutation;

public sealed class AuthorCreateType: InputObjectGraphType
{
    public AuthorCreateType()
    {
        Field<NonNullGraphType<StringGraphType>>("name");
    }
}