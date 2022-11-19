using GraphQL.Types;

namespace Practices.GraphQL.Web.Models.Book.Mutation;

public sealed class BookUpdateType : InputObjectGraphType
{
    public BookUpdateType()
    {
        Field<NonNullGraphType<IntGraphType>>("id");
        Field<StringGraphType>("description");
        Field<StringGraphType>("title");
        Field<IntGraphType>("authorId");
    }
}