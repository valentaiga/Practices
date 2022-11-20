namespace Practices.GraphQL.Client.Models.Responses.Author;

public class AuthorWrap<TResponse>
{
    public TResponse Author { get; set; }
}