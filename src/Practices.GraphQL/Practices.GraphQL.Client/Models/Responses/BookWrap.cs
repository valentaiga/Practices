namespace Practices.GraphQL.Client.Models.Responses;

public class BookWrap<TResponse>
{
    public TResponse Book { get; set; }
}