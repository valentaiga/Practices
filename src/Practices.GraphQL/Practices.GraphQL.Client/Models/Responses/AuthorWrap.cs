namespace Practices.GraphQL.Client.Models.Responses;

public class AuthorWrap<TResponse>
{
    public TResponse Author { get; set; }
}