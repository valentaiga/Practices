namespace Practices.GraphQL.Client.Models.Responses.Book;

public class BookWrap<TResponse>
{
    public TResponse Book { get; set; }
}