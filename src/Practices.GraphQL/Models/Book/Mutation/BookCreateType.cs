using GraphQL.Types;

namespace Practices.GraphQL.Models.Book.Mutation;

public sealed class BookCreateType : InputObjectGraphType
{
    public BookCreateType()
    {
        Field<NonNullGraphType<StringGraphType>>("description");
        Field<NonNullGraphType<StringGraphType>>("title");
        Field<NonNullGraphType<IntGraphType>>("authorId");
    }
}