using GraphQL.Types;

namespace Practices.GraphQL.GraphQL.Book.Mutation;

public sealed class BookInputType : InputObjectGraphType
{
    public BookInputType()
    {
        Field<NonNullGraphType<StringGraphType>>("description");
        Field<NonNullGraphType<StringGraphType>>("title");
    }
}